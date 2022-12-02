using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using RuddyRex.Lib;
using RuddyRex.Lib.Enums;
using RuddyRex.Lib.Exceptions.LexicalExceptions;
using RuddyRex.Lib.Models;
using RuddyRex.Lib.Models.TokenModels;
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

        [TestClass]
        public class LexerShouldTokenize
        {

            [TestMethod]
            public void WhenPassedParenthesisPairs()
            {
                List<IToken> expected = new()
                {
                    new TokenOperator() { Type = TokenType.OpeningParenthesis, Value = "(" },
                    new TokenOperator() { Type = TokenType.ClosingParenthesis, Value = ")" },
                };

                List<IToken> actual = Lexer.Tokenize("()");

                CollectionAssert.AreEqual(expected, actual);
            }
            [TestMethod]
            public void WhenPassedCurlyBracketsPairs()
            {
                List<IToken> expected = new()
                {
                    new TokenOperator() { Type = TokenType.OpeningCurlyBracket, Value = "{" },
                    new TokenOperator() { Type = TokenType.ClosingCurlyBracket, Value = "}" },
                };

                List<IToken> actual = Lexer.Tokenize("{}");

                CollectionAssert.AreEqual(expected, actual);
            }
            [TestMethod]
            public void WhenPassedSquareBracketsPairs()
            {
                List<IToken> expected = new()
                {
                    new TokenOperator() { Type = TokenType.OpeningSquareBracket, Value = "[" },
                    new TokenOperator() { Type = TokenType.ClosingSquareBracket, Value = "]" },
                };

                List<IToken> actual = Lexer.Tokenize("[]");

                CollectionAssert.AreEqual(expected, actual);
            }

            [TestMethod]
            [DataRow("[abcd]", "abcd")]
            [DataRow("[(abc]", "(abc")]
            //[DataRow("[abc]]", "abc]")] leave this otu for now
            public void WhenPassedAsCharacters(string input, string expected)
            {
                List<IToken> expectedTokens = new()
                {
                    new TokenOperator() { Type = TokenType.OpeningSquareBracket, Value = "[" },
                    new TokenCharacter() {Type = TokenType.CharacterLiteral, Character = expected[0]},
                    new TokenCharacter() {Type = TokenType.CharacterLiteral, Character = expected[1]},
                    new TokenCharacter() {Type = TokenType.CharacterLiteral, Character = expected[2]},
                    new TokenCharacter() {Type = TokenType.CharacterLiteral, Character = expected[3]},
                    new TokenOperator() { Type = TokenType.ClosingSquareBracket, Value = "]" },
                };
                IToken expectedToken = new TokenString() { Type = TokenType.StringLiteral, Value = expected };
                var tokens = Lexer.Tokenize(input);

                //Assert.AreEqual(expectedToken, token);
                CollectionAssert.AreEqual(expectedTokens, tokens);
            }
            [TestMethod]
            public void WhenPassedExpressionBetweenParenthsis()
            {

                List<IToken> expected = new()
            {
                new TokenOperator() { Type = TokenType.OpeningParenthesis, Value = "(" },
                new TokenKeyword() { Type = TokenType.KeywordIdentifier, Value = "Between"},
                new TokenOperator() { Type = TokenType.OpeningCurlyBracket, Value = "{" },
                new TokenNumber() {Type = TokenType.NumberLiteral, Value = 1},
                new TokenKeyword() { Type = TokenType.KeywordIdentifier, Value = "Till" },
                new TokenNumber() {Type = TokenType.NumberLiteral, Value = 3},
                new TokenOperator() { Type = TokenType.ClosingCurlyBracket, Value = "}" },
                new TokenKeyword() { Type = TokenType.KeywordIdentifier, Value = "Digit" },
                new TokenOperator() { Type = TokenType.ClosingParenthesis, Value = ")" }
            };

                List<IToken> actual = Lexer.Tokenize("(Between { 1 Till 3} Digit)");

                CollectionAssert.AreEqual(expected, actual);
            }

            [TestMethod]
            public void WhenPassedExpressionWithoutParenthsis()
            {

                List<IToken> expected = new()
            {
                new TokenKeyword() { Type = TokenType.KeywordIdentifier, Value = "Between"},
                new TokenOperator() { Type = TokenType.OpeningCurlyBracket, Value = "{" },
                new TokenNumber() {Type = TokenType.NumberLiteral, Value = 1},
                new TokenKeyword() { Type = TokenType.KeywordIdentifier, Value = "Till" },
                new TokenNumber() {Type = TokenType.NumberLiteral, Value = 3},
                new TokenOperator() { Type = TokenType.ClosingCurlyBracket, Value = "}" },
                new TokenKeyword() { Type = TokenType.KeywordIdentifier, Value = "Digit" },
            };

                List<IToken> actual = Lexer.Tokenize("Between { 1 Till 3} Digit");
                CollectionAssert.AreEqual(expected, actual);
            }
            [TestMethod]
            [DataRow("This is pure text")]
            [DataRow("This is pure text 12")]
            [DataRow("This is pure-text")]
            [DataRow("This `is pure+text")]
            [DataRow("This `is pure+text)")]
            [DataRow("This `is pure+text]")]
            [DataRow("This `is pure+text}")]
            public void Lexer_WhenPassedAString(string input)
            {

                List<IToken> expected = new()
                {
                    new TokenString() { Type = TokenType.StringLiteral, Value = input },
                };

                List<IToken> actual = Lexer.Tokenize($"\"{input}\"");

                CollectionAssert.AreEqual(expected, actual);
            }
        }
        [TestClass]
        public class LexerShouldIgnore
        {
            [TestMethod]
            [DataRow(" ")]
            [DataRow("\n ")]
            [DataRow("\t")]
            public void WhenPassedWhiteSpaces(string input)
            {
                int actual = Lexer.Tokenize(input).Count;
                Assert.AreEqual(0, actual);
            }
        }

        [TestClass]
        public class LexerThrowsException
        {
            [TestMethod]
            [ExpectedException(typeof(CharacterIsNotValidException))]
            [DataRow("%")]
            [DataRow("-")]
            [DataRow("+")]
            [DataRow("-")]
            public void WhenPassedInvalidCharacters(string invalid)
            {
                Lexer.Tokenize($"{invalid}Between {{ 1 Till 3}} Digit");
            }
        }
    }
}
