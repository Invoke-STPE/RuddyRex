using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using RuddyRex.Lib;
using RuddyRex.Lib.Enums;
using RuddyRex.Lib.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
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
        [DataRow("[ ]", "[", "]")]
        public void Lexer_ShouldTokenizeSymbolPairs(string input, string open, string close)
        {
            Lexer lexer = new Lexer(input);
            List<IToken> expected = new() 
            { 
                new TokenSymbol() { Type = TokenType.Symbol, Value = open },
                new TokenSymbol() { Type = TokenType.Symbol, Value = close },
            };

            List<IToken> actual = lexer.Tokenize();

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Lexer_ShouldTokenizeExpressionBetweenBrackets()
        {
            Lexer lexer = new Lexer("(Between { 1 Till 3} Digit)");

            List<IToken> expected = new()
            {
                new TokenSymbol() { Type = TokenType.Symbol, Value = "(" },
                new TokenName() { Type = TokenType.Name, Value = "Between"},
                new TokenSymbol() { Type = TokenType.Symbol, Value = "{" },
                new TokenNumber() {Type = TokenType.Number, Value = 1},
                new TokenName() { Type = TokenType.Name, Value = "Till" },
                new TokenNumber() {Type = TokenType.Number, Value = 3},
                new TokenSymbol() { Type = TokenType.Symbol, Value = "}" },
                new TokenName() { Type = TokenType.Name, Value = "Digit" },
                new TokenSymbol() { Type = TokenType.Symbol, Value = ")" }
            };

            List<IToken> actual = lexer.Tokenize();

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Lexer_ShouldTokenizeExpressionBetweenWithoutBrackets()
        {
            Lexer lexer = new Lexer("Between { 1 Till 3} Digit");

            List<IToken> expected = new()
            {
                new TokenName() { Type = TokenType.Name, Value = "Between"},
                new TokenSymbol() { Type = TokenType.Symbol, Value = "{" },
                new TokenNumber() {Type = TokenType.Number, Value = 1},
                new TokenName() { Type = TokenType.Name, Value = "Till" },
                new TokenNumber() {Type = TokenType.Number, Value = 3},
                new TokenSymbol() { Type = TokenType.Symbol, Value = "}" },
                new TokenName() { Type = TokenType.Name, Value = "Digit" },
            };

            List<IToken> actual = lexer.Tokenize();

            CollectionAssert.AreEqual(expected, actual);
        }
        //[TestMethod]
        //[DataRow("[abc]", "[", "a", "b", "c", "]")]
        //[DataRow(" [abc]", "[", "a", "b", "c", "]")]

        //public void Lexer_ShouldTokenizeCurlyBracketsWithNumbers_ReturnsListOfBracketsAndNumbers(string input, string open, string a, string b, string c, string close)
        //{
        //    List<IToken> expected = new()
        //    {
        //        new TokenSymbol() {Type = TokenType.Symbol, Value = open},
        //        new TokenCharacter() {Type = TokenType.Character, Value = Char.Parse(a)},
        //        new TokenCharacter() {Type = TokenType.Character, Value = Char.Parse(b)},
        //        new TokenCharacter() {Type = TokenType.Character, Value = Char.Parse(c)},
        //        new TokenSymbol() {Type = TokenType.Symbol, Value = close}
        //    };

        //    List<IToken> actual = Lexer.Tokenize(input);

        //    CollectionAssert.AreEqual(expected, actual);
        //}
    }
}
