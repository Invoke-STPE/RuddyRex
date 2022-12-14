using Microsoft.VisualStudio.TestTools.UnitTesting;
using NuGet.Frameworks;
using RuddyRex.Lib;
using RuddyRex.Lib.Enums;
using RuddyRex.Lib.Models;
using RuddyRex.Lib.Models.Interfaces;
using RuddyRex.Lib.Models.NodeModels;
using RuddyRex.Lib.Models.RegexModels;
using RuddyRex.Lib.Models.TokenModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RuddyRex.Tests
{
    [TestClass]
    public class RuddyToRegexTests
    {
        [TestClass]
        public class ConvertorShouldConvertExpression
        {
            [TestMethod]
            public void WhenPassedEmptyGroupExpression()
            {
                AbstractTree<INode> abstractTree = new();
                var groupNode = new GroupNode() { Type = NodeType.GroupExpression };
                abstractTree.Nodes.Add(groupNode);
                RegexType expected = RegexType.Group;

                RegexGroup actual = (RegexGroup)Traverser.ConvertTree(abstractTree).Nodes.First();
                Assert.AreEqual(expected, actual.Type);
            }
            [TestMethod]
            public void WhenPassedGroupExpression()
            {
                AbstractTree<INode> abstractTree = new();
                var groupNode = new GroupNode() { Type = NodeType.GroupExpression };
                groupNode.Nodes.Add(new CharacterRangeNode() { Type = NodeType.CharacterRange });
                abstractTree.Nodes.Add(groupNode);
                RegexType expectedType = RegexType.CharacterClass;

                RegexGroup regexGroup = (RegexGroup)Traverser.ConvertTree(abstractTree).Nodes.First();
                RegexType actual = regexGroup.Expressions.First().Type;
                Assert.AreEqual(expectedType, actual);
            }


            [TestMethod]
            public void WhenPassedEmptyCharacterRange()
            {
                AbstractTree<INode> abstractTree = new();
                var characterRange = new CharacterRangeNode() { Type = NodeType.CharacterRange};
                abstractTree.Nodes.Add(characterRange);
                RegexType expected = RegexType.CharacterClass;

                RegexCharacterClass actual = (RegexCharacterClass)Traverser.ConvertTree(abstractTree).Nodes.First();
                Assert.AreEqual(expected, actual.Type);
            }
            [TestMethod]
            public void WhenPassedCharacterRange()
            {

                AbstractTree<INode> abstractTree = new();
                var characterRange = new CharacterRangeNode() { Type = NodeType.CharacterRange };
                characterRange.Characters = new List<INode>
                {
                    new CharacterNode() { Type = NodeType.CharacterNode, Value = 'a' },
                    new CharacterNode() { Type = NodeType.CharacterNode, Value = 'b' }
                };
                abstractTree.Nodes.Add(characterRange);

                var expected = new List<IRegexNode>()
                {
                    new RegexChar() { Value = "a", Type = RegexType.Char, Kind = "simple", Symbol = 'a' },
                    new RegexChar() { Value = "b", Type = RegexType.Char, Kind = "simple", Symbol = 'b' },

                };

                var actual = (RegexCharacterClass)Traverser.ConvertTree(abstractTree).Nodes.First();

                CollectionAssert.AreEqual(expected, actual.Expressions);
            }

            [TestMethod]
            public void WhenPassedEmptyKeywordExpression()
            {
                AbstractTree<INode> abstractTree = new();
                var keywordNode = new KeywordNode() { Type = NodeType.KeywordExpression, Keyword = "Between", ValueType = "letter" };
                abstractTree.Nodes.Add(keywordNode);
                RegexRepetition expected = new()
                {
                    Type = RegexType.Repetition,
                    Expression = new RegexChar() { Type = RegexType.Char, Value = "[a-zA-Z]", Kind = "meta" },

                };

                var actual = (RegexRepetition)Traverser.ConvertTree(abstractTree).Nodes.First();

                Assert.AreEqual(expected, actual);
            }
            [TestMethod]
            public void WhenPassedBetweenTwoNumbersExpression()
            {
                AbstractTree<INode> abstractTree = new();
                var keywordNode = new KeywordNode() { Type = NodeType.KeywordExpression, Keyword = "Between", ValueType = "letter" };
                var rangeExpression = new RangeNode()
                {
                    Type = NodeType.RangeExpression,
                    Values = new List<IToken>()
                    { 
                        new TokenNumber() { Type = TokenType.NumberLiteral, Value = 1},
                        new TokenNumber() { Type = TokenType.NumberLiteral, Value = 2},
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
            public void WhenPassedBetweenOneNumberExpression()
            {
                AbstractTree<INode> abstractTree = new();
                var keywordNode = new KeywordNode() { Type = NodeType.KeywordExpression, Keyword = "Between", ValueType = "letter" };
                var rangeExpression = new RangeNode()
                {
                    Type = NodeType.RangeExpression,
                    Values = new List<IToken>()
                    {
                        new TokenNumber() { Type = TokenType.NumberLiteral, Value = 1},
                    }
                };
                keywordNode.Parameter = rangeExpression;
                abstractTree.Nodes.Add(keywordNode);
                RegexRepetition expected = new()
                {
                    Type = RegexType.Repetition,
                    Expression = new RegexChar() { Type = RegexType.Char, Value = "[a-zA-Z]", Kind = "meta" },
                    Quantifier = new RegexQuantifier() { Kind = "range", Type = RegexType.Quantifier, From = 1, To = 0 },
                };

                var actual = (RegexRepetition)Traverser.ConvertTree(abstractTree).Nodes.First();

                Assert.AreEqual(expected, actual);
            }
            [TestMethod]
            public void WhenPassedExactlyExpression()
            {
                AbstractTree<INode> abstractTree = new();
                var keywordNode = new KeywordNode() { Type = NodeType.KeywordExpression, Keyword = "Exactly", ValueType = "letter" };
                var rangeExpression = new RangeNode()
                {
                    Type = NodeType.RangeExpression,
                    Values = new List<IToken>()
                    {
                        new TokenNumber() { Type = TokenType.NumberLiteral, Value = 1},
                    }
                };
                keywordNode.Parameter = rangeExpression;
                abstractTree.Nodes.Add(keywordNode);
                RegexRepetition expected = new()
                {
                    Type = RegexType.Repetition,
                    Expression = new RegexChar() { Type = RegexType.Char, Value = "[a-zA-Z]", Kind = "meta" },
                    Quantifier = new RegexQuantifier() { Kind = "range", Type = RegexType.Quantifier, From = 1, To = 1 },
                };

                var actual = (RegexRepetition)Traverser.ConvertTree(abstractTree).Nodes.First();

                Assert.AreEqual(expected, actual);
            }

            [TestMethod]
            public void WhenPassedEmptyStringLiteral()
            {
                AbstractTree<INode> abstractTree = new();
                var stringNode = new StringNode() { Type = NodeType.StringLiteral, Value = "abc"};
                abstractTree.Nodes.Add(stringNode);
                RegexAlternative expected = new RegexAlternative() { Type = RegexType.Alternative };

                var actual = (RegexAlternative)Traverser.ConvertTree(abstractTree).Nodes.First();

                Assert.AreEqual(expected.Type, actual.Type);
            }

            [TestMethod]
            public void WhenPassedStringLiteral()
            {
                AbstractTree<INode> abstractTree = new();
                var stringNode = new StringNode() { Type = NodeType.StringLiteral, Value = "abc" };
                abstractTree.Nodes.Add(stringNode);
                List<RegexChar> expected = new List<RegexChar>() 
                {
                    new RegexChar() { Type = RegexType.Char, Kind = "simple", Value = "a", Symbol = 'a' },
                    new RegexChar() { Type = RegexType.Char, Kind = "simple", Value = "b", Symbol = 'b' },
                    new RegexChar() { Type = RegexType.Char, Kind = "simple", Value = "c", Symbol = 'c' },
                };

                var actual = (RegexAlternative)Traverser.ConvertTree(abstractTree).Nodes.First();

                CollectionAssert.AreEqual(expected, actual.Expressions);
            }
        }

        [TestClass]
        public class ConvertorShouldConvertCharacterRangeWithCharacters
        {

        }
    }
}
