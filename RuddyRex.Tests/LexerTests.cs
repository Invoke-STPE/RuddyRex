using Microsoft.VisualStudio.TestTools.UnitTesting;
using RuddyRex.Lib;
using RuddyRex.Lib.Enums;
using RuddyRex.Lib.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        [DataRow("{ }", "{", "}")]
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

        [TestMethod]
        [DataRow("[abc]")]
        [DataRow("[ abc ]")]
        [DataRow(" [ abc ]")]
        public void Lexer_ShouldTokenizeSquareBrackets_ReturnsListOfBracketsAndCharacter(string input, string open, string a, string b, string c, string close)
        {
            List<IToken> expected = new()
            {
                new TokenSymbol() {Type = TokenType.Symbol, Value = open},
                new TokenCharacter() {Type = TokenType.Character, Value = Char.Parse(a)},
                new TokenCharacter() {Type = TokenType.Character, Value = Char.Parse(b)},
                new TokenCharacter() {Type = TokenType.Character, Value = Char.Parse(c)},
                new TokenSymbol() {Type = TokenType.Symbol, Value = close}
            };

            List<IToken> actual = Lexer.Tokenize(input);

            CollectionAssert.AreEqual(expected, actual);
        }
    }
}
