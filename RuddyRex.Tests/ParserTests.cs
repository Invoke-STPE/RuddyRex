using Microsoft.VisualStudio.TestTools.UnitTesting;
using RuddyRex.Lib;
using RuddyRex.Lib.Enums;
using RuddyRex.Lib.Exceptions.SyntaxExceptions;
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
            [DataRow("Match ()", 1)]
            [DataRow("Match (())", 2)]
            [DataRow("Match ((()))()()", 5)]
            public void WhenPassedEmptyNestedGroupExpression(string input, int expected)
            {
                var tokens = Lexer.Tokenize(input);

                AbstractTree ast = Parser.ParseAST(tokens);

                int actual = CountNodes(ast.Nodes);

                Assert.AreEqual(expected, actual);
            }

            [TestMethod]
            [DataRow("Match (Between { 1 Till 2 } digit)")]
            [DataRow("Match (Between { 1 Till 2 } letter)")]
            public void WhenPassedGroupBetweenExpression_ReturnsGroupNode(string input)
            {
                List<IToken> tokens = Lexer.Tokenize(input);
                GroupNode expected = new GroupNode() { Type = NodeType.GroupExpression };

                var actual = Parser.ParseAST(tokens).Nodes.First();

                Assert.AreEqual(expected.Type, actual.Type);
            }
            [TestMethod]
            [DataRow("Match (Between { 1 Till 2 } digit)")]
            [DataRow("Match (Between { 1 Till 2 } letter)")]
            public void WhenPassedGroupBetweenExpression_ReturnsKeywordNode(string input)
            {
                List<IToken> tokens = Lexer.Tokenize(input);
                string valueType = input.Contains("digit") ? "digit" : "letter";
                KeywordNode expected = new KeywordNode() { Type = NodeType.KeywordExpression, Keyword = "Between", ValueType = valueType };

                GroupNode groupNode = (GroupNode)Parser.ParseAST(tokens).Nodes.First();
                var actual = groupNode.Nodes.First();
                Assert.AreEqual(expected, actual);
              
            }
            [TestMethod]
            [DataRow("Match (Between { 1 Till 2 } digit)")]
            [DataRow("Match (Between { 1 Till 2 } letter)")]
            public void WhenPassedGroupBetweenExpression_ReturnsRangeExpression(string input)
            {
                List<IToken> tokens = Lexer.Tokenize(input);

                RangeNode expected = new RangeNode() { Type = NodeType.RangeExpression };

                GroupNode groupNode = (GroupNode)Parser.ParseAST(tokens).Nodes.First();
                KeywordNode keyword = (KeywordNode)groupNode.Nodes.First();
                RangeNode actual = (RangeNode)keyword.Parameters.First();

                Assert.AreEqual(expected, actual);
                Assert.AreEqual(2, actual.Values.Count);
            }
        }
        [TestClass]
        public class ParserShouldParseExpression
        {
            [TestMethod]
            public void WhenPassedBetweenExpression()
            {
                // Todo: Implement proper list checking
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
            [TestMethod]
            [DataRow("Match Between { 1 Till 2 } digit")]
            [DataRow("Match Between { 1 Till 2 } letter")]
            public void WhenPassedBetweenExpression_ReturnsKeywordNode(string input)
            {
                List<IToken> tokens = Lexer.Tokenize(input);
                string valueType = input.Contains("digit") ? "digit" : "letter";
                KeywordNode expected = new KeywordNode() { Type = NodeType.KeywordExpression, Keyword = "Between", ValueType = valueType };

                KeywordNode actual = (KeywordNode)Parser.ParseAST(tokens).Nodes.First();
                //var actual = keywordNode.Parameters.First();
                Assert.AreEqual(expected, actual);

            }
            [TestMethod]
            [DataRow("Match Between { 1 Till 2 } digit")]
            [DataRow("Match Between { 1 Till 2 } letter")]
            public void WhenPassedGroupBetweenExpression_ReturnsRangeExpression(string input)
            {
                List<IToken> tokens = Lexer.Tokenize(input);

                RangeNode expected = new RangeNode() { Type = NodeType.RangeExpression };

                KeywordNode keyword = (KeywordNode)Parser.ParseAST(tokens).Nodes.First();
                RangeNode actual = (RangeNode)keyword.Parameters.First();

                Assert.AreEqual(expected, actual);
                Assert.AreEqual(2, actual.Values.Count);
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
            { // Todo: Fix unit test number two
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

            [TestMethod]
            [ExpectedException(typeof(InvalidRangeExpressionSyntax))]
            [DataRow("Match Between 3 Till 4 } digit")]
            [DataRow("Match Between { 3 Till 4  digit")]
            [DataRow("Match Between { 3 between 4  digit")]
            public void WhenPassedMissingCurlyBracket(string input)
            {
                var tokens = Lexer.Tokenize(input);

                Parser.ParseAST(tokens);
            }
            [TestMethod]
            [ExpectedException(typeof(InvalidKeywordException))]
            [DataRow("atch Between 3 Till 4 } digit")]
            [DataRow("Between { 3 Till 4  digit")]
            [DataRow("Exactly Between { 3 between 4  digit")]
            public void WhenPassedInvalidStartValue(string input)
            {
                var tokens = Lexer.Tokenize(input);
                Parser.ParseAST(tokens);
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
