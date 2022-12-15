using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using RuddyRex.Lib;
using RuddyRex.Lib.Enums;
using RuddyRex.Lib.Exceptions.SyntaxExceptions;
using RuddyRex.Lib.Models;
using RuddyRex.Lib.Models.Interfaces;
using RuddyRex.Lib.Models.NodeModels;
using RuddyRex.Lib.Models.TokenModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuddyRex.Tests
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
            //[DataRow("Match [[[]]]", 3)] // need to unit test this in my lexer 
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
                AbstractTree <INode> expectedTree = new AbstractTree<INode>()
                {
                    Type = "Match",
                    Nodes = new List<INode>()
                    {
                        new CharacterRangeNode()
                        {
                            Type = NodeType.CharacterRange,
                            Characters = new List<INode>()
                            {
                                new CharacterNode(){ Type = NodeType.CharacterNode, Value = 'a' },
                                new CharacterNode(){ Type = NodeType.CharacterNode, Value = 'b' },
                                new CharacterNode(){ Type = NodeType.CharacterNode, Value = 'c' },
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
                            Type = NodeType.GroupExpression,
                            Nodes = new List<INode>()
                            {
                                new CharacterRangeNode(){ Type = NodeType.CharacterRange },
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
                            Type = NodeType.CharacterRange,
                            Characters = new List<INode>()
                            {
                                new CharacterNode(){ Type = NodeType.CharacterNode, Value = '(' },
                                new CharacterNode(){ Type = NodeType.CharacterNode, Value = ')' },
                             
                            }
                        }
                    }
                };
                var actual = Parser.ParseTree(input);
                Assert.AreEqual(expectedTree, actual);
            }
        }

        [TestClass]
        public class ParserShouldThrowException // Spell it correctly.
        {
            [TestMethod]
            [ExpectedException(typeof(InvalidRangeExpression))]
            [DataRow("Match (")]
            [DataRow("Match ())")]
            [DataRow("Match ((())")]
            [DataRow("Match ()(")]
            [DataRow("Match (()")]
            public void WhenPassedUnevenParenthis(string input)
            {
                var tokens = Lexer.Tokenize(input);
                Parser.ParseTree(tokens);
            }
            [TestMethod]
            [ExpectedException(typeof(InvalidRangeExpression))]
            [DataRow("Match [")]
            [DataRow("Match ]")]
            [DataRow("Match []]")]
            public void WhenPassedUnevenSqaureBrackets(string input)
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
                count += CountCharacterRangeNodes(node.Characters);
            }

            return count;
        }


    }
}
