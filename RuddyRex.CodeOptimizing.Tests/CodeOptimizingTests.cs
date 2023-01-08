using RuddyRex.Core;
using RuddyRex.Core.Interfaces.RegexInterface;
using RuddyRex.Transformation;
using RuddyRex.Transformation.Models;

namespace RuddyRex.CodeOptimizing.Tests
{
    [TestClass]
    public class CodeOptimizingTests
    {
        [TestClass]
        public class ShouldOptimizeGroups
        {

            [TestMethod]
            public void WhenPassedRegexRepetition_RemovesUnnecessaryParenthese()
            {
                AbstractTree<IRegexNode> input = new AbstractTree<IRegexNode>()
                {
                    Type = "RegExp",
                    Nodes = new List<IRegexNode>()
                {
                    new RegexRepetition()
                    {
                        Expression = new RegexGroup()
                        {
                            Expressions = new List<IRegexNode>()
                            {
                                new RegexChar() { Kind = "simple", Symbol = 'a', Value = "a"}
                            }
                        },
                        Quantifier = new RegexQuantifier() { To = 0, Kind = "*", From = 0}
                    }
                }
                };

                AbstractTree<IRegexNode> expected = new AbstractTree<IRegexNode>()
                {
                    Type = "RegExp",
                    Nodes = new List<IRegexNode>()
                {
                    new RegexRepetition()
                    {
                        Expression =  new RegexChar() { Kind = "simple", Symbol = 'a', Value = "a"},
                        Quantifier = new RegexQuantifier() { To = 0, Kind = "*", From = 0}
                    }
                }
                };

                var actual = CodeOptimizer.OptimizeTree(input);
                Assert.AreEqual(expected, actual);
            }
            [TestMethod]
            public void WhenPassedRegexAlternative_RemovesUnnecessaryParenthese()
            {
                AbstractTree<IRegexNode> input = new AbstractTree<IRegexNode>()
                {
                    Type = "RegExp",
                    Nodes = new List<IRegexNode>()
                    {
                      new RegexAlternative()
                      {
                          Expressions = new List<IRegexNode>()
                          {
                                new RegexRepetition()
                                {
                                    Expression = new RegexGroup()
                                    {
                                        Expressions = new List<IRegexNode>()
                                        {
                                            new RegexChar() { Kind = "simple", Symbol = 'a', Value = "a"}
                                        }
                                    },
                                    Quantifier = new RegexQuantifier() { To = 0, Kind = "*", From = 0}
                                }
                          }
                      }
                    }
                };

                AbstractTree<IRegexNode> expected = new AbstractTree<IRegexNode>()
                {
                    Type = "RegExp",
                    Nodes = new List<IRegexNode>()
                    {
                        new RegexAlternative()
                        {
                            Expressions= new List<IRegexNode>()
                            {
                                new RegexRepetition()
                                {
                                    Expression =  new RegexChar() { Kind = "simple", Symbol = 'a', Value = "a"},
                                    Quantifier = new RegexQuantifier() { To = 0, Kind = "*", From = 0}
                                }
                            }
                        }
                    }
                };

                var actual = CodeOptimizer.OptimizeTree(input);
                Assert.AreEqual(expected, actual);
            }
            [TestMethod]
            public void WhenPassedRegexGroup_RemovesUnnecessaryParenthese()
            {
                AbstractTree<IRegexNode> input = new AbstractTree<IRegexNode>()
                {
                    Type = "RegExp",
                    Nodes = new List<IRegexNode>()
                    {
                        new RegexGroup()
                        {
                            Expressions = new List<IRegexNode>()
                            {
                                new RegexChar() { Kind = "simple", Symbol = 'a', Value = "a"}
                            }
                        },
                    }
                };

                AbstractTree<IRegexNode> expected = new AbstractTree<IRegexNode>()
                {
                    Type = "RegExp",
                    Nodes = new List<IRegexNode>()
                    {
                        new RegexChar() { Kind = "simple", Symbol = 'a', Value = "a"},
                    }
                };

                var actual = CodeOptimizer.OptimizeTree(input);
                Assert.AreEqual(expected, actual);
            }
        }

        [TestClass]
        public class ShouldOptimizeCharacterClass
        {
            [TestMethod]
            public void WhenPassedCharacterClass_RemovesUnnecessaryCharacterClass()
            {
                AbstractTree<IRegexNode> input = new AbstractTree<IRegexNode>()
                {
                    Type = "RegExp",
                    Nodes = new List<IRegexNode>()
                    {
                        new RegexCharacterClass()
                        {
                            Expressions = new List<IRegexNode>()
                            {
                                new RegexChar() { Kind = "simple", Symbol = 'a', Value = "a"}
                            }
                        },
                    }
                };
                AbstractTree<IRegexNode> expected = new AbstractTree<IRegexNode>()
                {
                    Type = "RegExp",
                    Nodes = new List<IRegexNode>()
                    {
                        new RegexChar() { Kind = "simple", Symbol = 'a', Value = "a"}
                    }

                };
                var actual = CodeOptimizer.OptimizeTree(input);
                Assert.AreEqual(expected, actual);
            }
            [TestMethod]
            public void WhenPassedAlternative_RemovesUnnecessaryCharacterClass()
            {
                AbstractTree<IRegexNode> input = new AbstractTree<IRegexNode>()
                {
                    Type = "RegExp",
                    Nodes = new List<IRegexNode>()
                    {
                       new RegexAlternative()
                       {
                           Expressions = new List<IRegexNode>()
                           {
                                new RegexCharacterClass()
                                {
                                    Expressions = new List<IRegexNode>()
                                    {
                                        new RegexChar() { Kind = "simple", Symbol = 'a', Value = "a"}
                                    }
                                },
                           }
                       }
                    }
                };
                AbstractTree<IRegexNode> expected = new AbstractTree<IRegexNode>()
                {
                    Type = "RegExp",
                    Nodes = new List<IRegexNode>()
                    {
                        new RegexAlternative()
                        {
                            Expressions =
                            {
                                new RegexChar() { Kind = "simple", Symbol = 'a', Value = "a"}
                            }
                        }

                    }

                };
                var actual = CodeOptimizer.OptimizeTree(input);
                Assert.AreEqual(expected, actual);
            }
            [TestMethod]
            public void WhenPassedRepetition_RemovesUnnecessaryCharacterClass()
            {
                AbstractTree<IRegexNode> input = new AbstractTree<IRegexNode>()
                {
                    Type = "RegExp",
                    Nodes = new List<IRegexNode>()
                    {
                       new RegexRepetition()
                       {
                           Expression = new RegexCharacterClass()
                            {
                                Expressions = new List<IRegexNode>()
                                {
                                    new RegexChar() { Kind = "simple", Symbol = 'a', Value = "a"}
                                }
                            },
                           Quantifier = new RegexQuantifier() { Kind = "*", To = 0, From = 0}
                           }
                       }
                };
                AbstractTree<IRegexNode> expected = new AbstractTree<IRegexNode>()
                {
                    Type = "RegExp",
                    Nodes = new List<IRegexNode>()
                    {
                        new RegexRepetition()
                        {
                            Expression = new RegexChar() { Kind = "simple", Symbol = 'a', Value = "a"},
                            Quantifier = new RegexQuantifier() { Kind = "*", To = 0, From = 0}
                        }

                    }

                };
                var actual = CodeOptimizer.OptimizeTree(input);
                Assert.AreEqual(expected, actual);
            }
        }
        [TestClass]
        public class ShouldOptimizeLiteralText
        {
            [TestMethod]
            public void WhenPassedLiteralCharAndPlusRangeInGroup_ShouldOptimizeExpression()
            {
                var input = new AbstractTree<IRegexNode>()
                {
                    Nodes = new List<IRegexNode>()
                    {
                        new RegexGroup()
                        {
                            Expressions =  new List<IRegexNode>()
                            {
                                 new RegexChar() { Kind = "simple", Symbol = 'a', Value = "a"},
                                 new RegexQuantifier(){ From = 0, To = 0, Kind = "+" },
                            }
                        }
                    }
                };

                var expected = new AbstractTree<IRegexNode>()
                {
                    Type = "RegExp",
                    Nodes = new List<IRegexNode>()
                    {
                        new RegexGroup()
                        {
                            Expressions =  new List<IRegexNode>()
                            {
                                 new RegexChar() { Kind = "simple", Symbol = 'a', Value = "a"},
                                 new RegexChar() { Kind = "simple", Symbol = 'a', Value = "a"},
                                 new RegexQuantifier(){ From = 0, To = 0, Kind = "*" },
                            }
                        }
                    }
                };

                var actual = CodeOptimizer.OptimizeTree(input);

                Assert.AreEqual(expected, actual);
            }
            [TestMethod]
            public void WhenPassedLiteralCharAndPlusRange_ShouldOptimizeExpression()
            {
                var input = new AbstractTree<IRegexNode>()
                {
                    Type = "RegExp",
                    Nodes = new List<IRegexNode>()
                    {
                        new RegexChar() { Kind = "simple", Symbol = 'a', Value = "a"},
                        new RegexQuantifier(){ From = 0, To = 0, Kind = "+" },
                    }
                };

                var expected = new AbstractTree<IRegexNode>()
                {
                    Type = "RegExp",
                    Nodes = new List<IRegexNode>()
                    {
                        new RegexChar() { Kind = "simple", Symbol = 'a', Value = "a"},
                        new RegexChar() { Kind = "simple", Symbol = 'a', Value = "a"},
                        new RegexQuantifier(){ From = 0, To = 0, Kind = "*" },
                    }
                };

                var actual = CodeOptimizer.OptimizeTree(input);

                Assert.AreEqual(expected, actual);
            }
        }
        [TestClass]
        public class OptimizerShouldNotOptimize
        {
            [TestMethod]
            public void WhenPassedLiteralTwoCharAndPlusRange_ShouldNotOptimizeExpression()
            {
                var input = new AbstractTree<IRegexNode>()
                {
                    Type = "RegExp",
                    Nodes = new List<IRegexNode>()
                    {
                        new RegexChar() { Kind = "simple", Symbol = 'a', Value = "a"},
                        new RegexChar() { Kind = "simple", Symbol = 'a', Value = "a"},
                        new RegexQuantifier(){ From = 0, To = 0, Kind = "+" },
                    }
                };

                var expected = new AbstractTree<IRegexNode>()
                {
                    Type = "RegExp",
                    Nodes = new List<IRegexNode>()
                    {
                        new RegexChar() { Kind = "simple", Symbol = 'a', Value = "a"},
                        new RegexChar() { Kind = "simple", Symbol = 'a', Value = "a"},
                        new RegexQuantifier(){ From = 0, To = 0, Kind = "+" },
                    }
                };

                var actual = CodeOptimizer.OptimizeTree(input);

                Assert.AreEqual(expected, actual);
            }
        }

    }
}