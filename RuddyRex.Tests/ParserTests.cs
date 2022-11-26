using Microsoft.VisualStudio.TestTools.UnitTesting;
using RuddyRex.Lib;
using RuddyRex.Lib.Enums;
using RuddyRex.Lib.Models;
using RuddyRex.Lib.Models.NodeModels;
using RuddyRex.Lib.Models.TokenModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuddyRex.Tests
{
    [TestClass]
    public class ParserTests
    {
        [TestClass]
        public class ParserSimpleTestCases
        {

            [TestMethod]
            public void Parser_ShouldReturnNumberLiteralNode()
            {
                List<IToken> tokens = new()
                {
                    new TokenNumber(){Type = TokenType.NumberLiteral, Value = 5}
                };

                INode expected = new NumberNode() { Type = NodeTypes.NumberLiteral, Value = 5 };

                AbstractTree abstractTree = Parser.ParseAST(tokens);

                CollectionAssert.Contains(abstractTree.Nodes, expected);

            }

            [TestMethod]
            public void Parser_ShouldReturnStringLiteralNode()
            {
                List<IToken> tokens = new()
                {
                    new TokenString(){Type = TokenType.StringLiteral, Value = "This is a test string"}
                };
                INode expected = new StringNode() { Type = NodeTypes.StringLiteral, Value = "This is a test string" };

                AbstractTree abstractTree = Parser.ParseAST(tokens);

                CollectionAssert.Contains(abstractTree.Nodes, expected);
            }
        }
       
    }
}
