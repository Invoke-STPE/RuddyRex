﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using RuddyRex.Lib;
using RuddyRex.ParserLayer;
using RuddyRex.ParserLayer.Interfaces;
using RuddyRex.ParserLayer.Models;
using RuddyRex.Transformation;
using RuddyRex.Transformation.Models;
using System.Linq.Expressions;

namespace RuddyRex.Tests
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

                RegexGroup actual = (RegexGroup)_convertor.ConvertToGroup(groupNode);
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

                RegexGroup actual = (RegexGroup)_convertor.ConvertToGroup(groupNode);
                Assert.AreEqual(expected, actual);
            }

            [TestMethod]
            public void WhenPassedGroupExpression_ReturnsAlternativeChars()
            {
                GroupNode groupNode = new GroupNode()
                {
                    Nodes = new List<INode>()
                    {
                        new StringNode() { Value = "Test"}
                    }
                };

                RegexGroup expected = new RegexGroup()
                {
                    Expressions = new List<IRegexNode>()
                    {
                        new RegexAlternative()
                        { 
                            Expressions = new List<IRegexNode>()
                            {
                                new RegexChar(){ Symbol = 'T', Value = "T"},
                                new RegexChar(){ Symbol = 'e', Value = "e"},
                                new RegexChar(){ Symbol = 's', Value = "s"},
                                new RegexChar(){ Symbol = 't', Value = "t"},
                            }
                        }
                    }
                };

                RegexGroup actual = (RegexGroup)_convertor.ConvertToGroup(groupNode);
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
                RangeNode rangeNode = new RangeNode()
                {
                    Values = new List<INode>()
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

                RegexQuantifier actual = (RegexQuantifier)_convertor.ConvertRange(rangeNode);

                Assert.AreEqual(expected, actual);
            }
            [TestMethod]
            public void WhenPassedOneNumberRangeExpression_ReturnsQuantifier()
            {
                RangeNode rangeNode = new RangeNode()
                {
                    Values = new List<INode>()
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

                RegexQuantifier actual = (RegexQuantifier)_convertor.ConvertRange(rangeNode);

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
                KeywordExpressionNode expression = new KeywordExpressionNode()
                {
                    Keyword = "Between",
                    ValueType = new KeywordNode() { Value = "letter" },
                    Parameter = new RangeNode()
                    {
                        Values = new List<INode>()
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
                    Expression = new RegexChar() { Symbol = 'a', Value = "a"}
                };

                RegexRepetition actual = (RegexRepetition)_convertor.ConvertKeyword(expression);
                Assert.AreEqual(expected, true);
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

                RegexChar actual = (RegexChar)_convertor.ConvertToChar(characterNode);

                Assert.AreEqual(expected, actual);
            }

            [TestMethod]
            public void WhenPassedCharacterRange_ReturnsCharacterClass()
            {
                CharacterRangeNode characterRange = new()
                {
                    Characters = new List<INode>()
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

                RegexCharacterClass actual = (RegexCharacterClass)_convertor.ConvertToCharacterClass(characterRange);

                Assert.AreEqual(expected, actual);
            }
        }

    }
}


