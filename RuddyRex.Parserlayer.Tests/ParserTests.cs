using RuddyRex.Core;
using RuddyRex.Core.Exceptions;
using RuddyRex.Core.Interfaces.NodeInterface;
using RuddyRex.LexerLayer;
using RuddyRex.ParserLayer;
using RuddyRex.ParserLayer.Models;

namespace RuddyRex.Parserlayer.Tests
{
    [TestClass]
    public class ParserTests
    {
        [TestClass]
        public class ParserShouldParseGroupExpression
        {
            [TestMethod]
            [DataRow("Match ()", 1)]
            [DataRow("Match (())", 2)]
            [DataRow("Match ((()))", 3)]
            public void WhenPassedEmptyNestedGroup_ReturnsGroupNode(string input, int expectedCount)
            {
                var tokens = Lexer.Tokenize(input);
                var ast = Parser.ParseTree(tokens);
                int actualCount = CountGroupNodes(ast.Nodes);
                Assert.AreEqual(expectedCount, actualCount);
            }

            [TestMethod]
            [DataRow("Match ()()", 2)]
            [DataRow("Match ()()()", 3)]
            [DataRow("Match ()()()()()", 5)]
            public void WhenPassedChainedEmptyGroups_ReturnsGroupNodes(string input, int expectedCount)
            {
                var tokens = Lexer.Tokenize(input);
                var ast = Parser.ParseTree(tokens);
                int actualCount = CountGroupNodes(ast.Nodes);
                Assert.AreEqual(expectedCount, actualCount);
            }
        }
        [TestClass]
        public class ParserShouldParseCharacterRanges
        {
            [TestMethod]
            [DataRow("Match []", 1)]
            [DataRow("Match [][]", 2)]
            public void WhenPassedEmptyCharacterRange(string input, int expectedCount)
            {
                var tokens = Lexer.Tokenize(input);
                var ast = Parser.ParseTree(tokens);
                int actualCount = ast.Nodes.Count();
                Assert.AreEqual(expectedCount, actualCount);
            }
            public void WhenPassedCharacterRange()
            {
                var tokens = Lexer.Tokenize("Match [abc]");
                AbstractTree<INode> expectedTree = new AbstractTree<INode>()
                {
                    Type = "Match",
                    Nodes = new List<INode>()
                    {
                        new CharacterRangeNode()
                        {
                            Nodes = new List<INode>()
                            {
                                new CharacterNode(){ Value = 'a' },
                                new CharacterNode(){ Value = 'b' },
                                new CharacterNode(){ Value = 'c' },
                            }
                        }
                    }
                };
                var actualTree = Parser.ParseTree(tokens);
                Assert.AreEqual(expectedTree, actualTree);
            }
            [TestMethod]
            public void WhenPassedCharacterRangeInsideGroup_ReturnsGroupAndRange()
            {
                var input = Lexer.Tokenize("Match ([])");
                AbstractTree<INode> expectedTree = new AbstractTree<INode>()
                {
                    Type = "Match",
                    Nodes = new List<INode>()
                    {
                        new GroupNode()
                        {
                            Nodes = new List<INode>()
                            {
                                new CharacterRangeNode(),
                            }
                        }
                    }
                };
                var actual = Parser.ParseTree(input);
                Assert.AreEqual(expectedTree, actual);
            }
            [TestMethod]
            public void WhenPassedParenthisInsideRange_ReturnsRangeAndCharacters()
            {
                var input = Lexer.Tokenize("Match [()]");
                AbstractTree<INode> expectedTree = new AbstractTree<INode>()
                {
                    Type = "Match",
                    Nodes = new List<INode>()
                    {
                        new CharacterRangeNode()
                        {
                            Nodes = new List<INode>()
                            {
                                new CharacterNode(){ Value = '(' },
                                new CharacterNode(){ Value = ')' },

                            }
                        }
                    }
                };
                var actual = Parser.ParseTree(input);
                Assert.AreEqual(expectedTree, actual);
            }
        }

        [TestClass]
        public class ParserShouldParseKeywordExpressions
        {
            [TestMethod]
            [DataRow("Match Between {1 Till 2} letters", "letters")]
            [DataRow("Match Between {1 Till 2} digit", "digit")]
            public void WhenPassedBetweenExpression(string input, string valueType)
            {
                var tokens = Lexer.Tokenize(input);
                AbstractTree<INode> expectedTree = new AbstractTree<INode>()
                {
                    Type = "Match",
                    Nodes = new List<INode>()
                    {
                        new KeywordExpressionNode()
                        {
                            Keyword = "Between",
                            ValueType = new KeywordNode() { Value = valueType},
                            Parameter = new RangeNode()
                            {

                                Nodes = new List<INode>()
                                {
                                    new NumberNode(){ Value = 1},
                                    new NumberNode(){ Value = 2}
                                }
                            }
                        },
                    }
                };
                var actualTree = Parser.ParseTree(tokens);

                Assert.AreEqual(expectedTree, actualTree);

            }
        }

        [TestClass]
        public class ParserShouldParseRangeException
        {
            [TestMethod]
            [DataRow("Match {1 Till 2}", 1, 2)]
            [DataRow("Match {12 Till 23}", 12, 23)]
            [DataRow("Match {0 Till 1}", 0, 1)]
            public void WhenPassedValidRangeExpression(string input, int numberA, int numberB)
            {
                var tokens = Lexer.Tokenize(input);
                AbstractTree<INode> expectedTree = new AbstractTree<INode>()
                {
                    Type = "Match",
                    Nodes = new List<INode>()
                    {
                        new RangeNode()
                        {
                            Nodes = new List<INode>()
                            {
                                new NumberNode(){ Value = numberA},
                                new NumberNode(){ Value = numberB}
                            }
                        }
                    }
                };

                var actualTree = Parser.ParseTree(tokens);

                Assert.AreEqual(expectedTree, actualTree);
            }
            [TestMethod]
            [DataRow("Match {1 Till }", 1)]
            [DataRow("Match {12 Till }", 12)]
            [DataRow("Match {0 Till }", 0)]
            public void WhenPassedValidRangeExpressionNumberToInfinity(string input, int numberA)
            {
                var tokens = Lexer.Tokenize(input);
                AbstractTree<INode> expectedTree = new AbstractTree<INode>()
                {
                    Type = "Match",
                    Nodes = new List<INode>()
                    {
                        new RangeNode()
                        {
                            Nodes = new List<INode>()
                            {
                                new NumberNode(){ Value = numberA},
                            }
                        }
                    }
                };

                var actualTree = Parser.ParseTree(tokens);

                Assert.AreEqual(expectedTree, actualTree);
            }
            [TestMethod]
            [DataRow("Match {1}", 1)]
            [DataRow("Match {12}", 12)]
            [DataRow("Match {0}", 0)]
            public void WhenPassedValidRangeExpressionExactly1Number(string input, int numberA)
            {
                var tokens = Lexer.Tokenize(input);
                AbstractTree<INode> expectedTree = new AbstractTree<INode>()
                {
                    Type = "Match",
                    Nodes = new List<INode>()
                    {
                        new RangeNode()
                        {
                            Nodes = new List<INode>()
                            {
                                new NumberNode(){ Value = numberA},
                            }
                        }
                    }
                };

                var actualTree = Parser.ParseTree(tokens);

                Assert.AreEqual(expectedTree, actualTree);
            }
            [TestMethod]
            public void WhenPassedTwoDifferentRanges()
            {
                var tokens = Lexer.Tokenize("Match []{1 Till 2}");
                AbstractTree<INode> expectedTree = new AbstractTree<INode>()
                {
                    Type = "Match",
                    Nodes = new List<INode>()
                    {
                        new CharacterRangeNode(),
                        new RangeNode()
                        {
                            Nodes = new List<INode>()
                            {
                                new NumberNode(){ Value = 1},
                                new NumberNode(){ Value = 2}
                            }
                        }
                    }
                };

                var actualTree = Parser.ParseTree(tokens);

                Assert.AreEqual(expectedTree, actualTree);
            }
        }

        [TestClass]
        public class ParserShouldParseStringLiterals
        {
            [TestMethod]
            [DataRow("Match \"Test\"", "Test")]
            [DataRow("Match \"\\Test\"", @"\Test")]
            public void WhenPassedStringTokenString_ReturnsStringNode(string input, string expectedText)
            {
                var tokens = Lexer.Tokenize(input);
                StringNode expected = new StringNode() { Value = expectedText };
                StringNode actual = (StringNode)Parser.ParseTree(tokens).Nodes.First();
                Assert.AreEqual(expected, actual);
            }

            [TestMethod]
            public void WhenPassedSpaceToken_ReturnsKeyword()
            {
                var tokens = Lexer.Tokenize("Match space");
                KeywordNode expected = new KeywordNode() { Value = "space" };
                KeywordNode actual = (KeywordNode)Parser.ParseTree(tokens).Nodes.First();
                Assert.AreEqual(expected, actual);
            }
        }

        [TestClass]
        public class ParserShouldParseChainedExpressions
        {
            [TestMethod]
            public void WhenPassedBetweenAndExactlyExpression_ReturnsParsedExpressions()
            {
                var input = Lexer.Tokenize("Match (Between {1 till 3} digit Exactly {1} letter)");
                AbstractTree<INode> expected = new()
                {
                    Type = "Match",
                    Nodes = new List<INode>()
                    {
                        new GroupNode()
                        {
                            Nodes = new List<INode>()
                            {
                                new KeywordExpressionNode()
                                {
                                    Keyword = "Between",
                                    Parameter = new RangeNode()
                                    {
                                        Nodes = new List<INode>()
                                        {
                                            new NumberNode() { Value = 1},
                                            new NumberNode() { Value = 3}
                                        }
                                    },
                                    ValueType = new KeywordNode(){ Value = "digit"}
                                },
                                new KeywordExpressionNode()
                                {
                                    Keyword = "Exactly",
                                    Parameter = new RangeNode()
                                    {
                                        Nodes = new List<INode>()
                                        {
                                            new NumberNode() { Value = 1},
                                        }
                                    },
                                    ValueType = new KeywordNode(){ Value = "letter"}
                                }
                            }
                        }
                    }
                };

                var actual = Parser.ParseTree(input);

                CollectionAssert.AreEqual(expected.Nodes, actual.Nodes);
            }

            [TestMethod]
            public void WhenPassedCharacterAndRange_ReturnsParsedCharacterRange()
            {
                var input = Lexer.Tokenize(@"Match ("","" { 0 till 1})");

                AbstractTree<INode> expected = new()
                {
                    Type = "Match",
                    Nodes = new List<INode>()
                    {
                        new GroupNode()
                        {
                            Nodes = new List<INode>()
                            {
                                new StringNode(){ Value = ","},
                                new RangeNode()
                                {
                                    Nodes = new List<INode>()
                                    {
                                        new NumberNode() { Value = 0},
                                        new NumberNode() { Value = 1},
                                    }
                                }
                            }
                        }

                    }
                };

                var actual = Parser.ParseTree(input);

                Assert.AreEqual(expected, actual);
            }

            [TestMethod]
            public void WhenPassedExpressionCharacterRange_ReturnsParsedExpression()
            {
                var tokens = Lexer.Tokenize(@"Match (Between {1 till 3} digit Exactly {1} letter "",""{0 till 1})");
                AbstractTree<INode> expected = new()
                {
                    Type = "Match",
                    Nodes = new List<INode>()
                    {
                        new GroupNode()
                        {
                            Nodes = new List<INode>()
                            {
                                new KeywordExpressionNode()
                                {
                                    Keyword = "Between",
                                    Parameter = new RangeNode()
                                    {
                                        Nodes = new List<INode>()
                                        {
                                            new NumberNode() { Value = 1},
                                            new NumberNode() { Value = 3}
                                        }
                                    },
                                    ValueType = new KeywordNode(){ Value = "digit"}
                                },
                                new KeywordExpressionNode()
                                {
                                    Keyword = "Exactly",
                                    Parameter = new RangeNode()
                                    {
                                        Nodes = new List<INode>()
                                        {
                                            new NumberNode() { Value = 1},
                                        }
                                    },
                                    ValueType = new KeywordNode(){ Value = "letter"}
                                },
                                new StringNode(){ Value = ","},
                                new RangeNode()
                                {
                                    Nodes = new List<INode>()
                                    {
                                        new NumberNode() { Value = 0},
                                        new NumberNode() { Value = 1},
                                    }
                                }
                            }
                        }
                    }
                };

                var actual = Parser.ParseTree(tokens);

                Assert.AreEqual(expected, actual);
            }
        }

        [TestClass]
        public class ParserShouldThrowException
        {
            [TestMethod]
            [ExpectedException(typeof(ExpectedBracketException))]
            [DataRow("Match (")]
            [DataRow("Match ())")]
            [DataRow("Match ((())")]
            [DataRow("Match ()(")]
            [DataRow("Match (()")]
            public void WhenPassedUnevenParenthis(string input) // Spell it correctly.
            {
                var tokens = Lexer.Tokenize(input);
                Parser.ParseTree(tokens);
            }
            [TestMethod]
            [ExpectedException(typeof(ExpectedBracketException))]
            [DataRow("Match [abc")]
            [DataRow("Match ]")]
            [DataRow("Match []]")]
            [DataRow("Match [abc]]")]
            public void WhenPassedUnevenSqaureBrackets(string input)
            {
                var tokens = Lexer.Tokenize(input);
                Parser.ParseTree(tokens);
            }

            [TestMethod]
            [ExpectedException(typeof(InvalidKeywordException))]
            [DataRow("Match {1 beteen }")]
            [DataRow("Match {1 exctly }")]
            public void WhenPassedInvalidRangeExpressionKeyword(string input)
            {
                var tokens = Lexer.Tokenize(input);
                Parser.ParseTree(tokens);
            }
            [TestMethod]
            [ExpectedException(typeof(InvalidKeywordException))]
            [DataRow("Match Between { }")]
            [DataRow("Match Exactly { }")]
            public void WhenPassedEmptyRangeExpressionKeyword(string input)
            {
                var tokens = Lexer.Tokenize(input);
                Parser.ParseTree(tokens);
            }
            [TestMethod]
            [ExpectedException(typeof(InvalidKeywordException))]
            [DataRow("Match Between {1 2}")]
            public void WhenPassedTwoNumberRangeExpressionWithoutKeyword(string input)
            {
                var tokens = Lexer.Tokenize(input);
                Parser.ParseTree(tokens);
            }

        }

        private static int CountGroupNodes(List<INode> nodes)
        {
            int count = 0;
            if (nodes.Count == 0)
            {
                return count;
            }
            count += nodes.Count;
            foreach (GroupNode node in nodes)
            {
                count += CountGroupNodes(node.Nodes);
            }

            return count;
        }
        private static int CountCharacterRangeNodes(List<INode> nodes)
        {
            int count = 0;
            if (nodes.Count == 0)
            {
                return count;
            }
            count += nodes.Count;
            foreach (CharacterRangeNode node in nodes)
            {
                count += CountCharacterRangeNodes(node.Nodes);
            }

            return count;
        }

    }
}