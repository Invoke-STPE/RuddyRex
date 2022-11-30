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
        public class ParserShouldReturn
        {
            [TestMethod]
            public void TrueWhenPassedParenthesisPair()
            {

                List<IToken> tokens = new()
                {
                    new TokenOperator(){ Type = TokenType.OpeningParenthesis, Value = "("},
                    new TokenOperator(){ Type = TokenType.ClosingParenthesis, Value = ")"},
                };
                INode expected = new GroupNode() { Type = NodeType.GroupExpression, Nodes = new List<INode>() };

                AbstractTree ast = Parser.ParseAST(tokens);
                NodeType actual = ast.Nodes.First().Type;
                Assert.AreEqual(expected.Type, actual);
            }
            [TestMethod]
            [DataRow("()", 1)]
            [DataRow("(Between)", 1)]
            [DataRow("(())", 2)]
            [DataRow("((()))()()", 5)]
            public void TrueWhenPassedEmptyNestedGroupExpression(string input, int expected)
            {
                var tokens = Lexer.Tokenize(input);

                AbstractTree ast = Parser.ParseAST(tokens);

                int actual = CountNodes(ast.Nodes);

                Assert.AreEqual(expected, actual);
            }



        }
        [TestClass]
        public class ParserThrowsException
        {
            [TestMethod]
            [ExpectedException(typeof(UnbalancedBracketsException))]
            [DataRow("(()")]
            [DataRow("(()))")]
            public void WhenPassedUnbalancedBrackets(string input)
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
