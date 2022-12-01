using RuddyRex.Lib.Enums;
using RuddyRex.Lib.Exceptions.LexicalExceptions;
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
    public static class Lexer
    {

        private static Queue<char> _sourceCode;
        public static List<IToken> Tokenize(string sourceCode)
        {
            _sourceCode = new Queue<char>(sourceCode);
            List<IToken> tokens = new();
           
            while (_sourceCode.Count != 0)
            {
                char character = NextCharacter();
                IToken token;
                switch (character)
                {
                    case '(':
                        token = new TokenOperator() { Type = TokenType.OpeningParenthesis, Value = character.ToString() };
                        tokens.Add(token);
                        continue;
                    case ')':
                        token = new TokenOperator() { Type = TokenType.ClosingParenthesis, Value = character.ToString() };
                        tokens.Add(token);
                        continue;
                    case '"':
                        string stringValue = "";
                        while (PeekCharacer() is not '"' )
                        {
                            stringValue += NextCharacter();
                        }
                        tokens.Add(new TokenString() { Type = TokenType.StringLiteral, Value = stringValue });
                        NextCharacter();
                        continue;
                    case '[':
                        token = new TokenOperator() { Type = TokenType.OpeningSquareBracket, Value = character.ToString() };
                        tokens.Add(token);
                        continue;
                    case ']':
                        token = new TokenOperator() { Type = TokenType.ClosingSquareBracket, Value = character.ToString() };
                        tokens.Add(token);
                        continue;
                    case '{':
                        token = new TokenOperator() { Type = TokenType.OpeningCurlyBracket, Value = character.ToString() };
                        tokens.Add(token);
                        continue;
                    case '}':
                        token = new TokenOperator() { Type = TokenType.ClosingCurlyBracket, Value = character.ToString() };
                        tokens.Add(token);
                        continue;
                    case var isWhitespace when new Regex("\\s").IsMatch(isWhitespace.ToString()):
                        continue;
                    case var isLetter when new Regex("[a-zA-Z]").IsMatch(isLetter.ToString()):
                        string letters = character.ToString();
                        while (char.IsLetter(PeekCharacer()))
                        {
                            letters += NextCharacter();
                        }
                        tokens.Add(new TokenKeyword() { Type = TokenType.KeywordIdentifier, Value = letters });
                        continue;
                    case var isNumber when new Regex("[0-9]").IsMatch(isNumber.ToString()):
                        string number = character.ToString();
                        while (char.IsDigit(PeekCharacer()))
                        {
                            number += NextCharacter(); ;
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

        private static char NextCharacter()
        {
            if (_sourceCode.TryDequeue(out char result))
            {
                return result;
            }
            return ' ';
        }

        private static char PeekCharacer()
        {
            return _sourceCode.TryPeek(out char result) ? result : ' ';
        }
    }
}
