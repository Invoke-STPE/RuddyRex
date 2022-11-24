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
                        letters += _sourceCode[_posInSourceCode];
                    }
                    tokens.Add(new TokenKeyword() { Type = TokenType.Name, Value = letters });
                    continue;
                }
                if (character.IsNumber())
                {
                    string number = character.ToString();
                    while (GetNextCharacter().IsNumber())
                    {
                        number += _sourceCode[_posInSourceCode];
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
                        letters += _sourceCode[_posInSourceCode];
                    }
                    tokens.Add(new TokenString() { Type = TokenType.String, Value = letters });
                    IncrementIndex();
                    continue;
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

        private char GetNextCharacter()
        {
            IncrementIndex();
            return _posInSourceCode > _maxSourceCodeLength ? ' ' : _sourceCode[_posInSourceCode];
        }
    }
}
