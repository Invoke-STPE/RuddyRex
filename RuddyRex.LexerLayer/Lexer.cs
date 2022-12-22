using RuddyRex.LexerLayer.Models;
using RuddyRex.LexerLayer.Exceptions;

using System.Text.RegularExpressions;
namespace RuddyRex.LexerLayer;

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
                    while (PeekCharacter() is not '"' )
                    {
                        stringValue += NextCharacter();
                    }
                    tokens.Add(new TokenString() { Value = stringValue });
                    NextCharacter();
                    continue;
                case '[':
                    token = new TokenOperator() { Type = TokenType.OpeningSquareBracket, Value = character.ToString() };
                    tokens.Add(token);
                    while (PeekCharacter() is not ']' && Char.IsWhiteSpace(PeekCharacter()) == false)
                    {
                        TokenCharacter tokenCharacter = new() { Character = NextCharacter() };
                        tokens.Add(tokenCharacter);
                    }    
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
                    while (char.IsLetterOrDigit(PeekCharacter()))
                    {
                        letters += NextCharacter();
                    }
                    tokens.Add(new TokenKeyword() { Value = letters });
                    continue;
                case var isNumber when new Regex("[0-9]").IsMatch(isNumber.ToString()):
                    string number = character.ToString();
                    while (char.IsDigit(PeekCharacter()))
                    {
                        number += NextCharacter(); ;
                    }
                    tokens.Add(new TokenNumber() { Value = Int32.Parse(number) });
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

    private static char PeekCharacter()
    {
        return _sourceCode.TryPeek(out char result) ? result : ' ';
    }
}
