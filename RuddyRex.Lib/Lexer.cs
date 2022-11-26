using RuddyRex.Lib.Enums;
using RuddyRex.Lib.Exceptions;
using RuddyRex.Lib.Extensions;
using RuddyRex.Lib.Helpers;
using RuddyRex.Lib.Models;
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
        private int _index;
        private int _maxStringLength = 0;
        private readonly string _input = "";

        public Lexer(string input)
        {
            _maxStringLength = input.Length - 1;
            _input = input;
        }
        public List<IToken> Tokenize()
        {
            _maxStringLength = _input.Length -1;
            List<IToken> tokens = new();
            
            while (_index <= _maxStringLength)
            {
                IToken token;
                char character = _input[_index];
                switch (character)
                {
                    case '(':
                        token = new TokenOperator() { Type = TokenType.Operator, Value = character.ToString() };
                        tokens.Add(token);
                        IncrementIndex();
                        continue;
                    case ')':
                        token = new TokenOperator() { Type = TokenType.Operator, Value = character.ToString() };
                        tokens.Add(token);
                        IncrementIndex();
                        continue;
                    case '"':
                        string stringValue = "";
                        while (GetNextCharacter().IsQuote() == false)
                        {
                            stringValue += _input[_index];
                        }
                        tokens.Add(new TokenString() { Type = TokenType.StringLiteral, Value = stringValue });
                        IncrementIndex();
                        continue;
                    case '[':
                        token = new TokenOperator() { Type = TokenType.Operator, Value = character.ToString() };
                        tokens.Add(token);
                        IncrementIndex();
                        continue;
                    case ']':
                        token = new TokenOperator() { Type = TokenType.Operator, Value = character.ToString() };
                        tokens.Add(token);
                        IncrementIndex();
                        continue;
                    case '{':
                        token = new TokenOperator() { Type = TokenType.Operator, Value = character.ToString() };
                        tokens.Add(token);
                        IncrementIndex();
                        continue;
                    case '}':
                        token = new TokenOperator() { Type = TokenType.Operator, Value = character.ToString() };
                        tokens.Add(token);
                        IncrementIndex();
                        continue;
                    case '|':
                        token = new TokenOperator() { Type = TokenType.Operator, Value = character.ToString() };
                        tokens.Add(token);
                        IncrementIndex();
                        continue;
                    case var isWhitespace when new Regex("\\s").IsMatch(isWhitespace.ToString()):
                        IncrementIndex();
                        continue;
                    case var isLetter when new Regex("[a-zA-Z]").IsMatch(isLetter.ToString()):
                        string letters = character.ToString();
                        while (GetNextCharacter().IsLetter())
                        {
                            letters += _input[_index];
                        }
                        tokens.Add(new TokenKeyword() { Type = TokenType.KeywordIdentifier, Value = letters });
                        continue;
                    case var isNumber when new Regex("[0-9]").IsMatch(isNumber.ToString()):
                        string number = character.ToString();
                        while (GetNextCharacter().IsNumber())
                        {
                            number += _input[_index];
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
            if (_index <= _maxStringLength)
            {
                _index++;
            }
        }

        private char GetNextCharacter()
        {
            IncrementIndex();
            return _index > _maxStringLength ? ' ' : _input[_index];
        }
    }
}
