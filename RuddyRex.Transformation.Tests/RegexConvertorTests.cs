using RuddyRex.Core.Interfaces.NodeInterface;
using RuddyRex.Core.Interfaces.RegexInterface;
using RuddyRex.Core.Types;
using RuddyRex.ParserLayer.Models;
using RuddyRex.Transformation.Models;

namespace RuddyRex.Transformation.Tests
{
    [TestClass]
    public class RegexConvetorTests
    {
        protected RegexConvertorVisitor _convertor;

        [TestInitialize]
        public void InitializeTests()
        {
            _convertor = new RegexConvertorVisitor();
        }

        [TestClass]
        public class ConvertorShouldConvertGroup : RegexConvetorTests
        {
            [TestMethod]
            public void WhenPassedEmptyGroupNode_ReturnRegexGroup()
            {
                GroupNode groupNode = new GroupNode();

                RegexType expected = RegexType.Group;

                RegexGroup actual = (RegexGroup)_convertor.ConvertGroup(groupNode);
                Assert.AreEqual(expected, actual.Type);
            }

            [TestMethod]
            public void WhenPassedGroupExpression_ReturnsSingleExpressionChar()
            {
                GroupNode groupNode = new GroupNode()
                {
                    Nodes = new List<INode>()
                    {
                        new StringNode() { Value = "a"}
                    }
                };

                RegexGroup expected = new RegexGroup()
                {
                    Expressions = new List<IRegexNode>()
                   {
                       new RegexChar(){ Symbol = 'a', Value = "a"}
                   }
                };

                RegexGroup actual = (RegexGroup)_convertor.ConvertGroup(groupNode);
                Assert.AreEqual(expected, actual);
            }

           
        }

        [TestClass]
        public class ConvertorShouldConvertString : RegexConvetorTests
        {
            [TestMethod]
            public void WhenPassedStringNode_ReturnsRegexChar()
            {
                StringNode input = new StringNode() { Value = "T" };
                RegexChar expected = new() { Kind = "simple", Symbol = 'T', Value = "T" };

                RegexChar actual = (RegexChar)_convertor.ConvertString(input);

                Assert.AreEqual(expected, actual);
            }
        }

        [TestClass]
        public class ConvertorShouldConvertRange : RegexConvetorTests
        {
            [TestMethod]
            public void WhenPassedTwoNumberRangeExpression_ReturnsQuantifier()
            {
                RegexConvertorVisitor convertor = new RegexConvertorVisitor(new KeywordExpressionNode() { Keyword = "Between" });

                RangeNode rangeNode = new RangeNode()
                {
                    Nodes = new List<INode>()
                    {
                        new NumberNode() {Value = 1},
                        new NumberNode() {Value = 2}
                    }
                };
                RegexQuantifier expected = new RegexQuantifier()
                {
                    From = 1,
                    To = 2,
                    Kind = "Range"
                };

                RegexQuantifier actual = (RegexQuantifier)convertor.ConvertRange(rangeNode);

                Assert.AreEqual(expected, actual);
            }
            [TestMethod]
            public void WhenPassedOneNumberRangeExpression_ReturnsQuantifier()
            {
                RegexConvertorVisitor convertor = new RegexConvertorVisitor(new KeywordExpressionNode() { Keyword = "Exactly" });
                RangeNode rangeNode = new RangeNode()
                {
                    Nodes = new List<INode>()
                    {
                        new NumberNode() {Value = 1}
                    }
                };
                RegexQuantifier expected = new RegexQuantifier()
                {
                    From = 1,
                    To = 1,
                    Kind = "Range"
                };

                RegexQuantifier actual = (RegexQuantifier)convertor.ConvertRange(rangeNode);

                Assert.AreEqual(expected, actual);
            }

            [TestMethod]
            public void WhenPassedReservedNumbers_ReturnsAsteriskQuantifier()
            {
                RegexConvertorVisitor convertor = new RegexConvertorVisitor(new KeywordExpressionNode() { Keyword = "Between" });
                RangeNode input = new RangeNode()
                {
                    Nodes = new List<INode>()
                    {
                        new NumberNode() { Value = 0 }
                    }
                };

                RegexQuantifier expected = new RegexQuantifier()
                {
                    Kind = "*"
                };

                RegexQuantifier actual = (RegexQuantifier)convertor.ConvertRange(input);

                Assert.AreEqual(expected, actual);
            }
            [TestMethod]
            public void WhenPassedReservedNumbers_ReturnsPlusQuantifier()
            {
                RegexConvertorVisitor convertor = new RegexConvertorVisitor(new KeywordExpressionNode() { Keyword = "Between" });

                RangeNode input = new RangeNode()
                {
                    Nodes = new List<INode>()
                    {
                        new NumberNode() { Value = 1 }
                    }
                };

                RegexQuantifier expected = new RegexQuantifier()
                {
                    Kind = "+"
                };

                RegexQuantifier actual = (RegexQuantifier)convertor.ConvertRange(input);

                Assert.AreEqual(expected, actual);
            }
            [TestMethod]
            public void WhenPassedReservedNumbers_ReturnsQuestionMarkQuantifier()
            {
                RegexConvertorVisitor convertor = new RegexConvertorVisitor(new KeywordExpressionNode() { Keyword = "Between" });

                RangeNode input = new RangeNode()
                {
                    Nodes = new List<INode>()
                    {
                        new NumberNode() { Value = 0 },
                        new NumberNode() { Value = 1 },

                    }
                };

                RegexQuantifier expected = new RegexQuantifier()
                {
                    Kind = "?"
                };

                RegexQuantifier actual = (RegexQuantifier)convertor.ConvertRange(input);

                Assert.AreEqual(expected, actual);
            }
        }

        [TestClass]
        public class ConvertorShouldConvertExpression : RegexConvetorTests
        {
            // [a-zA-Z]{6,89} <-- try this 
            [TestMethod]
            public void WhenPassedBetweenExpressionLetterType_ReturnsRepetition()
            {
                KeywordExpressionNode betweenExpression = new KeywordExpressionNode()
                {
                    Keyword = "Between",
                    ValueType = new KeywordNode() { Value = "letter" },
                    Parameter = new RangeNode()
                    {
                        Nodes = new List<INode>()
                        {
                            new NumberNode() { Value = 1},
                            new NumberNode() { Value = 2}
                        }
                    }
                };
                RegexRepetition expected = new()
                {
                    Quantifier = new RegexQuantifier()
                    {
                        From = 1,
                        To = 2,
                        Kind = "Range"
                    },
                    Expression = new RegexCharacterClass()
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
                };

                RegexRepetition actual = (RegexRepetition)_convertor.ConvertKeywordExpression(betweenExpression);
                Assert.AreEqual(expected, actual);
            }
            [TestMethod]
            public void WhenPassedBetweenExpressionDigitType_ReturnsRepetition()
            {
                KeywordExpressionNode betweenExpression = new KeywordExpressionNode()
                {
                    Keyword = "Between",
                    ValueType = new KeywordNode() { Value = "digit" },
                    Parameter = new RangeNode()
                    {
                        Nodes = new List<INode>()
                        {
                            new NumberNode() { Value = 1},
                            new NumberNode() { Value = 2}
                        }
                    }
                };
                RegexRepetition expected = new()
                {
                    Quantifier = new RegexQuantifier()
                    {
                        From = 1,
                        To = 2,
                        Kind = "Range"
                    },
                    Expression = new RegexCharacterClass()
                    {
                        Expressions = new List<IRegexNode>()
                        {
                            new RegexClassRange()
                            {
                                From = new RegexChar() { Value = "0", Symbol = '0'},
                                To = new RegexChar() {Value = "9", Symbol = '9'},
                            }
                        }
                    }
                };

                RegexRepetition actual = (RegexRepetition)_convertor.ConvertKeywordExpression(betweenExpression);
                Assert.AreEqual(expected, actual);
            }
            [TestMethod]
            public void WhenPassedExactlyExpressionLetterType_ReturnsRepetition()
            {
                KeywordExpressionNode betweenExpression = new KeywordExpressionNode()
                {
                    Keyword = "Exactly",
                    ValueType = new KeywordNode() { Value = "letter" },
                    Parameter = new RangeNode()
                    {
                        Nodes = new List<INode>()
                        {
                            new NumberNode() { Value = 1}
                        }
                    }
                };
                RegexRepetition expected = new()
                {
                    Quantifier = new RegexQuantifier()
                    {
                        From = 1,
                        To = 1,
                        Kind = "Range"
                    },
                    Expression = new RegexCharacterClass()
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
                };

                RegexRepetition actual = (RegexRepetition)_convertor.ConvertKeywordExpression(betweenExpression);
                Assert.AreEqual(expected, actual);
            }
            [TestMethod]
            public void WhenPassedExactlyExpressionDigitType_ReturnsRepetition()
            {
                KeywordExpressionNode betweenExpression = new KeywordExpressionNode()
                {
                    Keyword = "Exactly",
                    ValueType = new KeywordNode() { Value = "digit" },
                    Parameter = new RangeNode()
                    {
                        Nodes = new List<INode>()
                        {
                            new NumberNode() { Value = 1}
                        }
                    }
                };
                RegexRepetition expected = new()
                {
                    Quantifier = new RegexQuantifier()
                    {
                        From = 1,
                        To = 1,
                        Kind = "Range"
                    },
                    Expression = new RegexCharacterClass()
                    {
                        Expressions = new List<IRegexNode>()
                        {
                            new RegexClassRange()
                            {
                                From = new RegexChar() { Value = "0", Symbol = '0'},
                                To = new RegexChar() {Value = "9", Symbol = '9'},
                            }
                        }
                    }
                };

                RegexRepetition actual = (RegexRepetition)_convertor.ConvertKeywordExpression(betweenExpression);
                Assert.AreEqual(expected, actual);
            }
        }

        [TestClass]
        public class ConvertorShouldConvertCharacterRange : RegexConvetorTests
        {
            [TestMethod]
            public void WhenPassedSingleCharacterNode_ReturnsRegexChar()
            {
                CharacterNode characterNode = new CharacterNode() { Value = 'a' };
                RegexChar expected = new() { Value = "a", Symbol = 'a' };

                RegexChar actual = (RegexChar)_convertor.ConvertChar(characterNode);

                Assert.AreEqual(expected, actual);
            }

            [TestMethod]
            public void WhenPassedCharacterRange_ReturnsCharacterClass()
            {
                CharacterRangeNode characterRange = new()
                {
                    Nodes = new List<INode>()
                    {
                        new CharacterNode() { Value = 'a' },
                        new CharacterNode() { Value = 'b' },
                        new CharacterNode() { Value = 'c' },
                    }
                };

                RegexCharacterClass expected = new()
                {
                    Expressions = new List<IRegexNode>()
                    {
                        new RegexChar(){ Symbol = 'a', Value = "a" },
                        new RegexChar(){ Symbol = 'b', Value = "b" },
                        new RegexChar(){ Symbol = 'c', Value = "c" },
                    }
                };

                RegexCharacterClass actual = (RegexCharacterClass)_convertor.ConvertCharacterClass(characterRange);

                Assert.AreEqual(expected, actual);
            }
        }

        [TestClass]
        public class ConvertorShouldConvertKeyword : RegexConvetorTests
        {
            [TestMethod]
            public void WhenPassedSingleKeywordNode_ReturnsRegexChar()
            {
                KeywordNode keyword = new KeywordNode() { Value = "space" };
                RegexChar expected = new() { Value = " ", Symbol = ' ' };

                RegexChar actual = (RegexChar)_convertor.ConvertKeyword(keyword);

                Assert.AreEqual(expected, actual);
            }
        }



    }
}