//using RuddyRex.Lib.Enums;
//using RuddyRex.Lib.Models.Interfaces;
//using RuddyRex.Lib.Models.NodeModels;
//using RuddyRex.Lib.Models.RegexModels;
//using RuddyRex.Lib.Models.TokenModels;
//using RuddyRex.Lib.Models;
//using RuddyRex.Lib;
//using System;
//using RuddyRex.Lib.Models.RuddyRex.NodeModels;
//using Newtonsoft.Json.Linq;

//namespace RuddyRex.Tests
//{
//    [TestClass]
//    public class TraverserShouldReturnCorrectASTStructure
//    {
//        [TestMethod]
//        public void WhenPassedBetweenOneNumberExpression()
//        {
//            AbstractTree<INode> abstractTree = new();
//            var keywordNode = new KeywordExpressionNode() { Keyword = "Between", ValueType = new KeywordNode() { Value = "Letter" } };
//            var rangeExpression = new RangeNode()
//            {
//                Values = new List<INode>()
//                {
//                    new NumberNode() { Value = 1},
//                }
//            };
//            keywordNode.Parameter = rangeExpression;

//            abstractTree.Nodes.Add(keywordNode);
//            RegexRepetition expected = new()
//            {
//                Type = RegexType.Repetition,
//                Expression = new RegexChar() { Type = RegexType.Char, Value = "[a-zA-Z]", Kind = "meta" },
//                Quantifier = new RegexQuantifier() { Kind = "range", Type = RegexType.Quantifier, From = 1, To = 0 },
//            };

//            var actual = (RegexRepetition)Traverser.ConvertTree(abstractTree).Nodes.First();

//            Assert.AreEqual(expected, actual);
//        }
//        [TestMethod]
//        public void WhenPassedBetweenNumbersExpression()
//        {
//            AbstractTree<INode> abstractTree = new();
//            var keywordNode = new KeywordExpressionNode() { Keyword = "Between", ValueType = new KeywordNode() { Value = "Letter" } };
//            var rangeExpression = new RangeNode()
//            {
//                Values = new List<INode>()
//                {
//                    new NumberNode() { Value = 1},
//                    new NumberNode() { Value = 3},
//                }
//            };
//            keywordNode.Parameter = rangeExpression;
//            abstractTree.Nodes.Add(keywordNode);
//            RegexRepetition expected = new()
//            {
//                Type = RegexType.Repetition,
//                Expression = new RegexChar() { Type = RegexType.Char, Value = "[a-zA-Z]", Kind = "meta" },
//                Quantifier = new RegexQuantifier() { Kind = "range", Type = RegexType.Quantifier, From = 1, To = 3 },
//            };

//            var actual = (RegexRepetition)Traverser.ConvertTree(abstractTree).Nodes.First();

//            Assert.AreEqual(expected, actual);
//        }
//        [TestMethod]
//        public void WhenPassedExactlyExpression()
//        {
//            AbstractTree<INode> abstractTree = new();
//            var keywordNode = new KeywordExpressionNode() { Keyword = "Exactly", ValueType = new KeywordNode() { Value = "Letter" } };
//            var rangeExpression = GenerateRangeNode();
//            keywordNode.Parameter = rangeExpression;
//            abstractTree.Nodes.Add(keywordNode);
//            RegexRepetition expected = new()
//            {
//                Type = RegexType.Repetition,
//                Expression = new RegexChar() { Type = RegexType.Char, Value = "[a-zA-Z]", Kind = "meta" },
//                Quantifier = new RegexQuantifier() { Kind = "range", Type = RegexType.Quantifier, From = 1, To = 1 },
//            };

//            var actual = (RegexRepetition)Traverser.ConvertTree(abstractTree).Nodes.First();

//            Assert.AreEqual(expected, actual);
//        }
//        [TestMethod]
//        public void WhenPassedMultilineExpression()
//        {
//            AbstractTree<INode> abstractTree = new();
//            for (int i = 0; i < 2; i++) // Generate two between expressions
//            {
//                KeywordExpressionNode keywordNode = GenerateKeywordNode();
//                keywordNode.Parameter = GenerateRangeNode();
//                abstractTree.Nodes.Add(keywordNode); 
//            }
//            AbstractTree<IRegexNode> expected = new() 
//            {
//                Type = "RegExp",
//                Nodes = new List<IRegexNode>() 
//                {
//                    new RegexRepetition()
//                    {
//                        Type = RegexType.Repetition,
//                        Expression = new RegexChar() { Type = RegexType.Char},
//                        Quantifier = new RegexQuantifier() { Kind = "range"},
//                    },
//                     new RegexRepetition()
//                    {
//                        Type = RegexType.Repetition,
//                        Expression = new RegexChar() { Type = RegexType.Char},
//                        Quantifier = new RegexQuantifier() { Kind = "range"},
//                    }
//                }
//            };
//            var actual = Traverser.ConvertTree(abstractTree);
//            // Assert AST type
//            Assert.AreEqual(expected.Type, actual.Type);
//            // Assert RegexRepetition
//            Assert.AreEqual(expected.Nodes[0].Type, actual.Nodes[0].Type); // First element
//            Assert.AreEqual(expected.Nodes[1].Type, actual.Nodes[1].Type); // Second element 
//        }

//        [TestMethod]
//        public void WhenPassedGroupAndRange()
//        {
//            string input = "Match [a]{0 Till}";
//            var tokens = Lexer.Tokenize(input);

//            var ast = Parser.ParseTree(tokens);

//            Assert.IsTrue(true);

//            var expected = new RegexGroup()
//            {
//                Type = RegexType.Group,
//                Expressions = new List<IRegexNode>()
//                {
//                    new RegexRepetition()
//                    {
//                        Type = RegexType.Repetition,
//                        Expression = new RegexCharacterClass()
//                        {
//                            Type = RegexType.CharacterClass,
//                            Expressions = new List<IRegexNode>()
//                            {
//                                new RegexChar() { Kind = "simple", Symbol = 'a', Type = RegexType.Char, Value = "a" },
//                            }
//                        },
//                        Quantifier = new RegexQuantifier() { Type = RegexType.Quantifier, Kind = "*", From = 0, To = 0 },
//                    }
//                }
//            };
            

//            //var actualTree = Traverser.ConvertTree(abstractTree);

//            //RegexGroup actualGroup = (RegexGroup)actualTree.Nodes[0];
//            //Assert.AreEqual(expected.Type, actualGroup.Type);

//            //RegexRepetition actualRepetition = (RegexRepetition)actualGroup.Expressions[0];
//            //RegexRepetition expectedRepetition = (RegexRepetition)expected.Expressions[0];
//            //Assert.AreEqual(expectedRepetition.Expression, actualRepetition.Expression);
//            //Assert.AreEqual(expectedRepetition.Quantifier, actualTypes.Quantifier);
//        }

//        private static RangeNode GenerateRangeNode()
//        {
//            return new RangeNode()
//            {
//                Values = new List<INode>()
//                {
//                    new NumberNode(),
//                }
//            };
//        }
//        private static KeywordExpressionNode GenerateKeywordNode()
//        {
//            return new KeywordExpressionNode() { Keyword = "Between", ValueType = new KeywordNode() { Value = "Letter" } };
//        }
//    }

//}
