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

                AbstractTree ast = Parser.Parse(tokens);

                int actual = CountNodes(ast.Nodes);

                Assert.AreEqual(expected, actual);
            }

            [TestMethod]
            [DataRow("Match (Between { 1 Till 2 } digit)")]
            [DataRow("Match (Between { 1 Till 2 } letter)")]
            [DataRow("Match (Exactly { 1 } letter)")]
            [DataRow("Match (Exactly { 1 } digit)")]
            public void WhenPassedGroupBetweenExpression_ReturnsGroupNode(string input)
            {
                List<IToken> tokens = Lexer.Tokenize(input);
                GroupNode expected = new GroupNode() { Type = NodeType.GroupExpression };

                var actual = Parser.Parse(tokens).Nodes.First();

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

                GroupNode groupNode = (GroupNode)Parser.Parse(tokens).Nodes.First();
                var actual = groupNode.Nodes.First();
                Assert.AreEqual(expected, actual);
              
            }
            [TestMethod]
            [DataRow("Match (Between { 1 Till 2 } digit)", 2)]
            [DataRow("Match (Between { 1 Till 2 } letter)", 2)]
            [DataRow("Match (Exactly { 1 } letter)", 1)]
            [DataRow("Match (Exactly { 1 } digit)", 1)]
            public void WhenPassedGroupBetweenExpression_ReturnsRangeExpression(string input, int expectedCount)
            {
                List<IToken> tokens = Lexer.Tokenize(input);

                RangeNode expected = new RangeNode() { Type = NodeType.RangeExpression };

                GroupNode groupNode = (GroupNode)Parser.Parse(tokens).Nodes.First();
                KeywordNode keyword = (KeywordNode)groupNode.Nodes.First();
                RangeNode actual = (RangeNode)keyword.Parameters.First();

                Assert.AreEqual(expected, actual);
                Assert.AreEqual(expectedCount, actual.Values.Count);
            }
        }
        [TestClass]
        public class ParserShouldParseExpression
        {
            [TestMethod]
            [DataRow("Match Between { 1 Till 2 } digit")]
            [DataRow("Match Between { 1 Till 2 } letter")]
            public void WhenPassedBetweenExpression_ReturnsKeywordNode(string input)
            {
                List<IToken> tokens = Lexer.Tokenize(input);
                string valueType = input.Contains("digit") ? "digit" : "letter";
                KeywordNode expected = new KeywordNode() { Type = NodeType.KeywordExpression, Keyword = "Between", ValueType = valueType };

                KeywordNode actual = (KeywordNode)Parser.Parse(tokens).Nodes.First();
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

                KeywordNode keyword = (KeywordNode)Parser.Parse(tokens).Nodes.First();
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
                Parser.Parse(tokens);

            }

            [TestMethod]
            [ExpectedException(typeof(InvalidKeywordException))]
            [DataRow("atch")]
            public void WhenPassedInvalidKeyword(string input)
            {
                var token = Lexer.Tokenize(input);
                Parser.Parse(token);
            }

            [TestMethod]
            [ExpectedException(typeof(InvalidRangeExpressionSyntax))]
            [DataRow("Match Between 3 Till 4 } digit")]
            [DataRow("Match Between { 3 Till 4  digit")]
            [DataRow("Match Between { 3 between 4  digit")]
            public void WhenPassedMissingCurlyBracket(string input)
            {
                var tokens = Lexer.Tokenize(input);

                Parser.Parse(tokens);
            }
            [TestMethod]
            [ExpectedException(typeof(InvalidKeywordException))]
            [DataRow("atch Between 3 Till 4 } digit")]
            [DataRow("Between { 3 Till 4  digit")]
            [DataRow("Exactly Between { 3 between 4  digit")]
            public void WhenPassedInvalidStartValue(string input)
            {
                var tokens = Lexer.Tokenize(input);
                Parser.Parse(tokens);
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
