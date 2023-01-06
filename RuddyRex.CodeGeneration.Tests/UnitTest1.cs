using RuddyRex.Core;
using RuddyRex.Core.Interfaces.RegexInterface;
using RuddyRex.Transformation.Models;

namespace RuddyRex.CodeGeneration.Tests
{
    [TestClass]
    public class CodeGeneratorTests
    {
        [TestClass]
        public class ShouldGenerateGroup
        {
            [TestMethod]
            public void WhenPassedGroup()
            {
                AbstractTree<IRegexNode> input = new()
                {
                    Type = "RegExp",
                    Nodes = new List<IRegexNode>()
                    {
                        new RegexGroup(),
                    }
                };
                string expected = "()";

                string actual = CodeGenerator.GenerateCode(input);

                Assert.AreEqual(expected, actual);
            }
            [TestMethod]
            public void WhenPassedNestedGroup()
            {
                AbstractTree<IRegexNode> input = new()
                {
                    Type = "RegExp",
                    Nodes = new List<IRegexNode>()
                    {
                        new RegexGroup()
                        {
                            Expressions = new List<IRegexNode>()
                            {
                                new RegexChar() { Kind = "simple", Symbol = 'a', Value = "a" },
                                new RegexGroup()
                                {
                                    Expressions = new List<IRegexNode>()
                                    {
                                        new RegexChar() { Kind = "simple", Symbol = 'a', Value = "a" }
                                    }
                                },
                            }
                        }

                    }
                };
                string expected = "(a(a))";

                string actual = CodeGenerator.GenerateCode(input);

                Assert.AreEqual(expected, actual);
            }
            [TestMethod]
            public void WhenPassedGroupCharacterClass()
            {
                AbstractTree<IRegexNode> input = new()
                {
                    Type = "RegExp",
                    Nodes = new List<IRegexNode>()
                {
                    new RegexGroup()
                    {
                        Expressions = new List<IRegexNode>()
                        {
                            new RegexCharacterClass()
                            {
                                Expressions = new List<IRegexNode>()
                                {
                                     new RegexChar() { Kind = "simple", Symbol = 'a', Value = "a" },
                                     new RegexChar() { Kind = "simple", Symbol = 'b', Value = "b" },
                                }
                            }
                        }
                    }

                }
                };
                string expected = "([ab])";

                string actual = CodeGenerator.GenerateCode(input);

                Assert.AreEqual(expected, actual);
            }
        }
        [TestClass]
        public class ShouldGenerateCharacterClass
        {
            [TestMethod]
            public void WhenPassedCharacterClass()
            {
                AbstractTree<IRegexNode> input = new()
                {
                    Type = "RegExp",
                    Nodes = new List<IRegexNode>()
                    {
                        new RegexCharacterClass()
                        {
                            Expressions = new List<IRegexNode>()
                            {
                                new RegexChar() { Kind = "simple", Symbol = 'a', Value = "a" },
                                new RegexChar() { Kind = "simple", Symbol = 'b', Value = "b" },
                            }
                        },
                    }
                };
                string expected = "[ab]";

                string actual = CodeGenerator.GenerateCode(input);

                Assert.AreEqual(expected, actual);
            }
        }
        [TestClass]
        public class ShouldGenerateCharacter
        {
            [TestMethod]
            public void WhenPassedCharacter()
            {
                AbstractTree<IRegexNode> input = new()
                {
                    Type = "RegExp",
                    Nodes = new List<IRegexNode>()
                    {
                        new RegexChar() { Kind = "simple", Symbol = 'a', Value = "a" },
                    }
                };
                string expected = "a";

                string actual = CodeGenerator.GenerateCode(input);

                Assert.AreEqual(expected, actual);
            }
        }
        [TestClass]
        public class ShouldGenerateRepetition
        {
            [TestMethod]
            public void WhenPassedRepetition()
            {
                AbstractTree<IRegexNode> input = new()
                {
                    Type = "RegExp",
                    Nodes = new List<IRegexNode>()
                    {
                        new RegexRepetition()
                        {
                            Expression = new RegexChar() { Kind = "simple", Symbol = 'a', Value = "a" },
                            Quantifier = new RegexQuantifier() { Kind = "Range", From = 2, To = 3 },
                        }
                    }
                };
                string expected = "a{2,3}";

                string actual = CodeGenerator.GenerateCode(input);

                Assert.AreEqual(expected, actual);
            }
        }
        [TestClass] // Need many more unit tests
        public class ShouldGenerateQuantifier
        {
            [TestMethod]
            public void WhenPassedQuantifier()
            {
                AbstractTree<IRegexNode> input = new()
                {
                    Type = "RegExp",
                    Nodes = new List<IRegexNode>()
                    {
                        new RegexQuantifier() { Kind = "Range", From = 2, To = 3 },
                    }
                };
                string expected = "{2,3}";

                string actual = CodeGenerator.GenerateCode(input);

                Assert.AreEqual(expected, actual);
            }
            [TestMethod]
            public void WhenPassedPlusQuantifier()
            {
                AbstractTree<IRegexNode> input = new()
                {
                    Type = "RegExp",
                    Nodes = new List<IRegexNode>()
                    {
                        new RegexChar() { Kind = "simple", Symbol = 'a', Value = "a" },
                        new RegexQuantifier() { Kind = "+"},
                    }
                };
                string expected = "a+";

                string actual = CodeGenerator.GenerateCode(input);

                Assert.AreEqual(expected, actual);
            }
            [TestMethod]
            public void WhenPassedAsteriskQuantifier()
            {
                AbstractTree<IRegexNode> input = new()
                {
                    Type = "RegExp",
                    Nodes = new List<IRegexNode>()
                    {
                        new RegexChar() { Kind = "simple", Symbol = 'a', Value = "a" },
                        new RegexQuantifier() { Kind = "*"},
                    }
                };
                string expected = "a*";

                string actual = CodeGenerator.GenerateCode(input);

                Assert.AreEqual(expected, actual);
            }
            [TestMethod]
            public void WhenPassedQuestionMarkQuantifier()
            {
                AbstractTree<IRegexNode> input = new()
                {
                    Type = "RegExp",
                    Nodes = new List<IRegexNode>()
                    {
                        new RegexChar() { Kind = "simple", Symbol = 'a', Value = "a" },
                        new RegexQuantifier() { Kind = "?"},
                    }
                };
                string expected = "a?";

                string actual = CodeGenerator.GenerateCode(input);

                Assert.AreEqual(expected, actual);
            }
        }

    }

    [TestClass]
    public class ShouldGenerateAlternative
    {
        [TestMethod]
        public void WhenPassedAlternative()
        {
            AbstractTree<IRegexNode> input = new()
            {
                Type = "RegExp",
                Nodes = new List<IRegexNode>()
                    {
                        new RegexAlternative()
                        {
                            Expressions = new List<IRegexNode>()
                            {
                                new RegexChar() { Kind = "simple", Symbol = 'a', Value = "a" },
                                new RegexChar() { Kind = "simple", Symbol = 'a', Value = "y" },
                            },

                        }
                    }
            };
            string expected = "ay";

            string actual = CodeGenerator.GenerateCode(input);

            Assert.AreEqual(expected, actual);
        }
    }

    [TestClass]
    public class ShouldGenerateClassRange
    {
        [TestMethod]
        public void WhenPassedClassRange()
        {
            AbstractTree<IRegexNode> input = new()
            {
                Type = "RegExp",
                Nodes = new List<IRegexNode>()
                    {
                        new RegexCharacterClass()
                        {
                            Expressions = new List<IRegexNode>()
                            {
                            new RegexClassRange()
                                {
                                    From = new RegexChar() { Value = "a-z", Symbol = 'a'},
                                    To = new RegexChar() {Value = "A-Z", Symbol = 'z'},
                                }
                            }
                        }
                    }
            };
            string expected = "[a-zA-Z]";

            string actual = CodeGenerator.GenerateCode(input);

            Assert.AreEqual(expected, actual);
        }
    }
}