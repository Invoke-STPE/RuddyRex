using Microsoft.VisualStudio.TestTools.UnitTesting;
using RuddyRex.Lib;
using RuddyRex.Lib.Enums;
using RuddyRex.Lib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuddyRex.Tests
{
    [TestClass]
    public class LexerTests
    {
        [TestMethod]
        [DataRow("()", "(", ")")]
        [DataRow("{}", "{", "}")]
        [DataRow("[]", "[", "]")]
        public void Lexer_ShouldTokenizeSymbolPairs_ReturnTokenizedSymbols(string input, string open, string close)
        {
            List<IToken> expected = new() 
            { 
                new TokenSymbol() { Type = TokenType.Symbol, Value = open },
                new TokenSymbol() { Type = TokenType.Symbol, Value = close },
            };

            List<IToken> actual = Lexer.Tokenize(input);

            CollectionAssert.AreEqual(expected, actual);
        }
    }
}
