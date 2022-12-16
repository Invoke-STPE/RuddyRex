using Microsoft.VisualStudio.TestTools.UnitTesting;
using RuddyRex.Lib;
using RuddyRex.Lib.Enums;
using RuddyRex.Lib.Models;
using RuddyRex.Lib.Models.Interfaces;
using RuddyRex.Lib.Models.NodeModels;
using RuddyRex.Lib.Models.RegexModels;
using RuddyRex.Lib.Models.RuddyRex.NodeModels;
using RuddyRex.Lib.Models.TokenModels;
using RuddyRex.Lib.Visitor;
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
        public class ConvertorShouldConvertExpression : RegexConvetorTests
        {
            [TestMethod]
            public void WhenPassedEmptyGroupExpression()
            {
                var groupNode = new GroupNode();
                RegexType expected = RegexType.Group;
                RegexGroup actual = (RegexGroup)_convertor.ConvertToGroup(groupNode);
                Assert.AreEqual(expected, actual.Type);
            }
            [TestMethod]
            public void WhenPassedRangeNode()
            {
                var characterRange = new RangeNode();
                RegexType expectedType = RegexType.Quantifier;
                var regexRange = (RegexQuantifier)_convertor.ConvertRange(characterRange);
                RegexType actual = regexRange.Type;
                Assert.AreEqual(expectedType, actual);
            }


            [TestMethod]
            public void WhenPassedEmptyCharacterRange()
            {
                var characterRange = new CharacterRangeNode();
                RegexType expected = RegexType.CharacterClass;

                RegexCharacterClass actual = (RegexCharacterClass)_convertor.ConvertToCharacterClass(characterRange);
                Assert.AreEqual(expected, actual.Type);
            }
            [TestMethod]
            public void WhenPassedCharacterRange()
            {
                var characterRange = new CharacterRangeNode();
                characterRange.Characters = new List<INode>
                {
                    new CharacterNode() { Value = 'a' },
                    new CharacterNode() { Value = 'b' }
                };

                var expected = new List<IRegexNode>()
                {
                    new RegexChar() { Value = "a", Type = RegexType.Char, Kind = "simple", Symbol = 'a' },
                    new RegexChar() { Value = "b", Type = RegexType.Char, Kind = "simple", Symbol = 'b' },

                };

                var actual = (RegexCharacterClass)_convertor.ConvertToCharacterClass(characterRange);

                CollectionAssert.AreEqual(expected, actual.Expressions);
            }


            [TestMethod]
            public void WhenPassedEmptyKeywordExpression()
            {
                var keywordNode = new KeywordNode() { Keyword = "Between", ValueType = "letter" };
                RegexRepetition expected = new()
                {
                    Type = RegexType.Repetition,
                    Expression = new RegexChar() { Type = RegexType.Char, Value = "[a-zA-Z]", Kind = "meta" },
                };
                var actual = (RegexRepetition)_convertor.ConvertKeyword(keywordNode);
                Assert.AreEqual(expected, actual);
            }
            [TestMethod]
            public void WhenPassedBetweenTwoNumbersExpression()
            {
                AbstractTree<INode> abstractTree = new();
                var keywordNode = new KeywordNode() { Keyword = "Between", ValueType = "letter" };
                var rangeExpression = new RangeNode()
                {
                    Values = new List<INode>()
                    { 
                        new NumberNode() { Value = 1},
                        new NumberNode() { Value = 2},
                    }
                };
                keywordNode.Parameter = rangeExpression;
                abstractTree.Nodes.Add(keywordNode);
                RegexRepetition expected = new()
                {
                    Type = RegexType.Repetition,
                    Expression = new RegexChar() { Type = RegexType.Char, Value = "[a-zA-Z]", Kind = "meta" },
                    Quantifier = new RegexQuantifier() { Kind = "range", Type = RegexType.Quantifier, From = 1, To = 2 },
                };

                var actual = (RegexRepetition)Traverser.ConvertTree(abstractTree).Nodes.First();

                Assert.AreEqual(expected, actual);
            }
            [TestMethod]
            public void WhenPassedStringLiteral()
            {
                var stringNode = new StringNode();
                List<RegexChar> expected = new List<RegexChar>()
                {
                    new RegexChar() { Type = RegexType.Char, Kind = "simple", Value = "a", Symbol = 'a' },
                    new RegexChar() { Type = RegexType.Char, Kind = "simple", Value = "b", Symbol = 'b' },
                    new RegexChar() { Type = RegexType.Char, Kind = "simple", Value = "c", Symbol = 'c' },
                };

                var actual = (RegexAlternative)_convertor.ConvertString(stringNode);

                CollectionAssert.AreEqual(expected, actual.Expressions);
            }
            [TestMethod]
            public void WhenPassedEmptyStringLiteral()
            {
                var stringNode = new StringNode();
                RegexAlternative expected = new RegexAlternative() { Type = RegexType.Alternative };

                var actual = (RegexAlternative)_convertor.ConvertString(stringNode);

                Assert.AreEqual(expected.Type, actual.Type);
            }
        }
    }
}
