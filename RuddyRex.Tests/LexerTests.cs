﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using RuddyRex.Lib;
using RuddyRex.Lib.Enums;
using RuddyRex.Lib.Exceptions;
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

        [TestClass]
        public class LexerTokenizeSymbolsTest
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
            [DataRow("|")]
            public void Lexer_ShouldTokenizeSingleSymbols(string symbol)
            {
                Lexer lexer = new Lexer(symbol);
                List<IToken> expected = new()
            {
                new TokenSymbol() { Type = TokenType.Symbol, Value = symbol },
            };

                List<IToken> actual = lexer.Tokenize();

                CollectionAssert.AreEqual(expected, actual);
            }
            [TestMethod]
            [DataRow(" ")]
            [DataRow("\n ")]
            [DataRow("\t")]
            public void Lexer_ShouldIgnoreWhiteSpaces(string input)
            {
                Lexer lexer = new(input);
                int actual = lexer.Tokenize().Count;
                Assert.AreEqual(0, actual);
            }
        }

        [TestClass]
        public class LexerTokenizeExpressionTests
        {
            [TestMethod]
            public void Lexer_ShouldTokenizeExpressionBetweenBrackets()
            {
                Lexer lexer = new Lexer("(Between { 1 Till 3} Digit)");

                List<IToken> expected = new()
            {
                new TokenSymbol() { Type = TokenType.Symbol, Value = "(" },
                new TokenKeyword() { Type = TokenType.Name, Value = "Between"},
                new TokenSymbol() { Type = TokenType.Symbol, Value = "{" },
                new TokenNumber() {Type = TokenType.Number, Value = 1},
                new TokenKeyword() { Type = TokenType.Name, Value = "Till" },
                new TokenNumber() {Type = TokenType.Number, Value = 3},
                new TokenSymbol() { Type = TokenType.Symbol, Value = "}" },
                new TokenKeyword() { Type = TokenType.Name, Value = "Digit" },
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
                new TokenKeyword() { Type = TokenType.Name, Value = "Between"},
                new TokenSymbol() { Type = TokenType.Symbol, Value = "{" },
                new TokenNumber() {Type = TokenType.Number, Value = 1},
                new TokenKeyword() { Type = TokenType.Name, Value = "Till" },
                new TokenNumber() {Type = TokenType.Number, Value = 3},
                new TokenSymbol() { Type = TokenType.Symbol, Value = "}" },
                new TokenKeyword() { Type = TokenType.Name, Value = "Digit" },
            };

                List<IToken> actual = lexer.Tokenize();
                CollectionAssert.AreEqual(expected, actual);
            }
            [TestMethod]
            public void Lexer_ShouldTokenizeAString()
            {
                Lexer lexer = new Lexer("\"This is pure text\"");

                List<IToken> expected = new()
            {
                new TokenString() { Type = TokenType.String, Value = "This is pure text" },
            };

                List<IToken> actual = lexer.Tokenize();

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
                Lexer lexer = new($"{invalid}Between {{ 1 Till 3}} Digit");
                lexer.Tokenize();
            }
        }
    }
}
