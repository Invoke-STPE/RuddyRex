using Microsoft.VisualStudio.TestTools.UnitTesting;
using RuddyRex.Lib;
using RuddyRex.Lib.Enums;
using RuddyRex.Lib.Exceptions;
using RuddyRex.Lib.Models;
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
        public class ParserShouldParseGroupException
        {
            [TestMethod]
            public void WhenPassedParenthesisPair()
            {

                List<IToken> tokens = new()
                {

                    new TokenKeyword(){ Type = TokenType.KeywordIdentifier, Value = "Match"},
                    new TokenOperator(){ Type = TokenType.OpeningParenthesis, Value = "("},
                    new TokenOperator(){ Type = TokenType.ClosingParenthesis, Value = ")"},
                };
                INode expected = new GroupNode() { Type = NodeType.GroupExpression, Nodes = new List<INode>() };

                AbstractTree ast = Parser.ParseAST(tokens);
                NodeType actual = ast.Nodes.First().Type;
                Assert.AreEqual(expected.Type, actual);
            }
            [TestMethod]
            [DataRow("Match ()", 1)]
            [DataRow("Match (Between)", 1)]
            [DataRow("Match (())", 2)]
            [DataRow("Match ((()))()()", 5)]
            public void WhenPassedEmptyNestedGroupExpression(string input, int expected)
            {
                var tokens = Lexer.Tokenize(input);

                AbstractTree ast = Parser.ParseAST(tokens);

                int actual = CountNodes(ast.Nodes);

                Assert.AreEqual(expected, actual);
            }
        }
        [TestClass]
        public class ParserShouldParseExpression
        {
            [TestMethod]
            public void WhenPassedBetweenExpression()
            {
                List<IToken> input = Lexer.Tokenize("Match Between { 1 Till 2 } digit");
                AbstractTree expected = new AbstractTree() { Type = "Match"};
                KeywordNode keywordNode = new KeywordNode() { Type = NodeType.KeywordExpression, Keyword = "Between", ValueType = "digit"};
                RangeNode rangeNode = new RangeNode() { Type = NodeType.RangeExpression };
                rangeNode.Values.Add(new TokenNumber() { Type = TokenType.NumberLiteral, Value = 1 });
                rangeNode.Values.Add(new TokenNumber() { Type = TokenType.NumberLiteral, Value = 2 });
                keywordNode.Parameters.Add(rangeNode);
                expected.Nodes.Add(keywordNode);

                var actual = Parser.ParseAST(input); // Skal laves efter frokost

                Assert.AreEqual(expected.Type, actual.Type);

                KeywordNode keywordExpected = (KeywordNode)expected.Nodes.First();
                KeywordNode keywordActual = (KeywordNode)actual.Nodes.First();
                Assert.AreEqual(keywordExpected, keywordActual);
                for (int i = 0; i < keywordActual.Parameters.Count; i++)
                {
                    Assert.AreEqual(keywordExpected.Parameters[i], keywordActual.Parameters[i]);
                }
                

            }
        }
        [TestClass]
        public class ParserShouldThrowException
        {
            [TestMethod]
            [ExpectedException(typeof(UnbalancedBracketsException))]
            [DataRow("Match (()")]
            [DataRow("Match (()))")]
            public void WhenPassedUnbalancedBrackets(string input)
            {
                var tokens = Lexer.Tokenize(input);
                Parser.ParseAST(tokens);

            }

            [TestMethod]
            [ExpectedException(typeof(InvalidKeywordException))]
            [DataRow("atch")]
            public void WhenPassedInvalidKeyword(string input)
            {
                var token = Lexer.Tokenize(input);
                Parser.ParseAST(token);
            }
        }

        private static int CountNodes(List<INode> nodes)
        {
            int count = 0;
            if (nodes.Count == 0)
            {
                return count;
            }
            count += nodes.Count;
            foreach (GroupNode node in nodes)
            {
                count += CountNodes(node.Nodes);
            }

            return count;
        }

    }
}
