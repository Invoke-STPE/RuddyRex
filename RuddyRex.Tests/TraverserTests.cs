﻿using RuddyRex.Lib.Enums;
using RuddyRex.Lib.Models.Interfaces;
using RuddyRex.Lib.Models.NodeModels;
using RuddyRex.Lib.Models.RegexModels;
using RuddyRex.Lib.Models.TokenModels;
using RuddyRex.Lib.Models;
using RuddyRex.Lib;


namespace RuddyRex.Tests
{
    [TestClass]
    public class TraverserShouldReturnCorrectASTStructure
    {
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
        public void WhenPassedBetweenNumbersExpression()
        {
            AbstractTree<INode> abstractTree = new();
            var keywordNode = new KeywordNode() { Type = NodeType.KeywordExpression, Keyword = "Between", ValueType = "letter" };
            var rangeExpression = new RangeNode()
            {
                Type = NodeType.RangeExpression,
                Values = new List<IToken>()
                {
                    new TokenNumber() { Type = TokenType.NumberLiteral, Value = 1},
                    new TokenNumber() { Type = TokenType.NumberLiteral, Value = 3},
                }
            };
            keywordNode.Parameter = rangeExpression;
            abstractTree.Nodes.Add(keywordNode);
            RegexRepetition expected = new()
            {
                Type = RegexType.Repetition,
                Expression = new RegexChar() { Type = RegexType.Char, Value = "[a-zA-Z]", Kind = "meta" },
                Quantifier = new RegexQuantifier() { Kind = "range", Type = RegexType.Quantifier, From = 1, To = 3 },
            };

            var actual = (RegexRepetition)Traverser.ConvertTree(abstractTree).Nodes.First();

            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void WhenPassedExactlyExpression()
        {
            AbstractTree<INode> abstractTree = new();
            var keywordNode = new KeywordNode() { Type = NodeType.KeywordExpression, Keyword = "Exactly", ValueType = "letter" };
            var rangeExpression = GenerateRangeNode();
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
        public void WhenPassedMultilineExpression()
        {
            AbstractTree<INode> abstractTree = new();
            for (int i = 0; i < 2; i++) // Generate two between expressions
            {
                KeywordNode keywordNode = GenerateKeywordNode();
                keywordNode.Parameter = GenerateRangeNode();
                abstractTree.Nodes.Add(keywordNode); 
            }
            AbstractTree<IRegexNode> expected = new() 
            {
                Type = "RegExp",
                Nodes = new List<IRegexNode>() 
                {
                    new RegexRepetition()
                    {
                        Type = RegexType.Repetition,
                        Expression = new RegexChar() { Type = RegexType.Char},
                        Quantifier = new RegexQuantifier() { Kind = "range"},
                    },
                     new RegexRepetition()
                    {
                        Type = RegexType.Repetition,
                        Expression = new RegexChar() { Type = RegexType.Char},
                        Quantifier = new RegexQuantifier() { Kind = "range"},
                    }
                }
            };
            var actual = Traverser.ConvertTree(abstractTree);
            // Assert AST type
            Assert.AreEqual(expected.Type, actual.Type);
            // Assert RegexRepetition
            Assert.AreEqual(expected.Nodes[0].Type, actual.Nodes[0].Type); // First element
            Assert.AreEqual(expected.Nodes[1].Type, actual.Nodes[1].Type); // Second element 
        }

        private static RangeNode GenerateRangeNode()
        {
            return new RangeNode()
            {
                Type = NodeType.RangeExpression,
                Values = new List<IToken>()
                {
                    new TokenNumber() { Type = TokenType.NumberLiteral, Value = 1},
                }
            };
        }
        private static KeywordNode GenerateKeywordNode()
        {
            return new KeywordNode() { Type = NodeType.KeywordExpression, Keyword = "Between", ValueType = "letter" };
        }
    }

}