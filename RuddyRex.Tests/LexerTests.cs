using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using RuddyRex.Lib;
using RuddyRex.Lib.Enums;
using RuddyRex.Lib.Exceptions;
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
        public class LexerCanTokenizeBasicCharacters
        {

            [TestMethod]
            public void Lexer_ShouldTokenizeParenthesisPairs()
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
            public void Lexer_ShouldTokenizeCurlyBracketsPairs()
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
            public void Lexer_ShouldTokenizeSquareBracketsPairs()
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
            [DataRow(" ")]
            [DataRow("\n ")]
            [DataRow("\t")]
            public void Lexer_ShouldIgnoreWhiteSpaces(string input)
            {
                int actual = Lexer.Tokenize(input).Count;
                Assert.AreEqual(0, actual);
            }
        }

        [TestClass]
        public class LexerTokenizeExpressionTests
        {
            [TestMethod]
            public void Lexer_ShouldTokenizeExpressionBetweenBrackets()
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
            public void Lexer_ShouldTokenizeExpressionBetweenWithoutBrackets()
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
            public void Lexer_ShouldTokenizeAString()
            {

                List<IToken> expected = new()
            {
                new TokenString() { Type = TokenType.StringLiteral, Value = "This is pure text" },
            };

                List<IToken> actual = Lexer.Tokenize("\"This is pure text\"");

                CollectionAssert.AreEqual(expected, actual);
            }
        }

        [TestClass]
        public class LexerThrowsExceptionsTests
        {
            [TestMethod]
            [ExpectedException(typeof(CharacterIsNotValidException))]
            [DataRow("%")]
            [DataRow("-")]
            [DataRow("+")]
            [DataRow("-")]
            public void Lexer_DoesNotRecognizeCharacter_ThrowsCharacterIsNotValidException(string invalid)
            {
                Lexer.Tokenize($"{invalid}Between {{ 1 Till 3}} Digit");
            }
        }
    }
}
