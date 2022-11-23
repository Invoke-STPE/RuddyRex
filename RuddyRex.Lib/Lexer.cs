using RuddyRex.Lib.Enums;
using RuddyRex.Lib.Exceptions;
using RuddyRex.Lib.Extensions;
using RuddyRex.Lib.Helpers;
using RuddyRex.Lib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                char character = _input[_index];

                if (character.IsSymbol())
                {
                    TokenSymbol symbol = new() { Type = TokenType.Symbol, Value = character.ToString()};
                    tokens.Add(symbol);
                    IncrementIndex();
                    continue;
                }
                if (character.IsLetter())
                {
                    string letters = character.ToString();
                    while (GetNextCharacter().IsLetter())
                    {
                        letters += _input[_index];
                    }
                    tokens.Add(new TokenName() { Type = TokenType.Name, Value = letters });
                    continue;
                }
                if (character.IsNumber())
                {
                    string number = character.ToString();
                    while (GetNextCharacter().IsNumber())
                    {
                        number += _input[_index];
                    }
                    tokens.Add(new TokenNumber() { Type = TokenType.Number, Value = Int32.Parse(number) });
                    continue;
                }
                if (character.IsWhiteSpace())
                {
                    IncrementIndex();
                    continue;
                }
                if (character.IsQuote())
                {
                    string letters = "";
                    while (GetNextCharacter().IsQuote() == false)
                    {
                        letters += _input[_index];
                    }
                    tokens.Add(new TokenString() { Type = TokenType.String, Value = letters });
                    IncrementIndex();
                    continue;
                }

                throw new CharacterIsNotValidException($"{character} Is not a valid character");
            }

            return tokens;
        }

        private bool IncrementIndex()
        {
            if (_index <= _maxStringLength)
            {
                _index++;
                return true;
            }

            return false;
        }

        private char GetNextCharacter()
        {
            bool result = IncrementIndex();
            return _index > _maxStringLength ? ' ' : _input[_index];
        }
    }
}
