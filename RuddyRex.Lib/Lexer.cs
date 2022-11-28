using RuddyRex.Lib.Enums;
using RuddyRex.Lib.Exceptions;
using RuddyRex.Lib.Models.TokenModels;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RuddyRex.Lib
{
    public class Lexer
    {
        private int _posInSourceCode;
        private int _maxSourceCodeLength = 0;
        private readonly string _sourceCode = "";

        public Lexer(string sourceCode)
        {
            _maxSourceCodeLength = sourceCode.Length - 1;
            _sourceCode = sourceCode;
        }
        public List<IToken> Tokenize()
        {
            _maxSourceCodeLength = _sourceCode.Length -1;
            List<IToken> tokens = new();
           
            while (_posInSourceCode <= _maxSourceCodeLength)
            {
                char character = _sourceCode[_posInSourceCode];
                IToken token;
                switch (character)
                {
                    case '(':
                        token = new TokenOperator() { Type = TokenType.OpeningParenthesis, Value = character.ToString() };
                        tokens.Add(token);
                        IncrementIndex();
                        continue;
                    case ')':
                        token = new TokenOperator() { Type = TokenType.ClosingParenthesis, Value = character.ToString() };
                        tokens.Add(token);
                        IncrementIndex();
                        continue;
                    case '"':
                        string stringValue = "";
                        while (NextCharacter() is not '"' )
                        {
                            stringValue += _sourceCode[_posInSourceCode];
                        }
                        tokens.Add(new TokenString() { Type = TokenType.StringLiteral, Value = stringValue });
                        IncrementIndex();
                        continue;
                    case '[':
                        token = new TokenOperator() { Type = TokenType.OpeningSquareBracket, Value = character.ToString() };
                        tokens.Add(token);
                        IncrementIndex();
                        continue;
                    case ']':
                        token = new TokenOperator() { Type = TokenType.ClosingSquareBracket, Value = character.ToString() };
                        tokens.Add(token);
                        IncrementIndex();
                        continue;
                    case '{':
                        token = new TokenOperator() { Type = TokenType.OpeningCurlyBracket, Value = character.ToString() };
                        tokens.Add(token);
                        IncrementIndex();
                        continue;
                    case '}':
                        token = new TokenOperator() { Type = TokenType.ClosingCurlyBracket, Value = character.ToString() };
                        tokens.Add(token);
                        IncrementIndex();
                        continue;
                    case var isWhitespace when new Regex("\\s").IsMatch(isWhitespace.ToString()):
                        IncrementIndex();
                        continue;
                    case var isLetter when new Regex("[a-zA-Z]").IsMatch(isLetter.ToString()):
                        string letters = character.ToString();
                        while (char.IsLetter(NextCharacter()))
                        {
                            letters += _sourceCode[_posInSourceCode];
                        }
                        tokens.Add(new TokenKeyword() { Type = TokenType.KeywordIdentifier, Value = letters });
                        continue;
                    case var isNumber when new Regex("[0-9]").IsMatch(isNumber.ToString()):
                        string number = character.ToString();
                        while (char.IsDigit(NextCharacter()))
                        {
                            number += _sourceCode[_posInSourceCode];
                        }
                        tokens.Add(new TokenNumber() { Type = TokenType.NumberLiteral, Value = Int32.Parse(number) });
                        continue;
                    default:
                        throw new CharacterIsNotValidException($"{character} Is not a valid character");
                }

                throw new CharacterIsNotValidException($"{character} Is not a valid character");
            }

            return tokens;
        }

        private void IncrementIndex()
        {
            if (_posInSourceCode <= _maxSourceCodeLength)
            {
                _posInSourceCode++;
            }
        }

        private char NextCharacter()
        {
            IncrementIndex();
            return _posInSourceCode > _maxSourceCodeLength ? ' ' : _sourceCode[_posInSourceCode];
        }
    }
}
