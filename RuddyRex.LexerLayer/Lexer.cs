using RuddyRex.LexerLayer.Models;
using RuddyRex.LexerLayer.Exceptions;
using System.Text.RegularExpressions;
using RuddyRex.Core.Interfaces.TokenInterfaces;
using RuddyRex.Core.Types;

namespace RuddyRex.LexerLayer;

public static class Lexer
{

    private static string _sourceCode;
    private static List<IToken> _tokens = new List<IToken>();
    private static int _startIndex = 0;
    private static int _index = 0;
    public static List<IToken> Tokenize(string sourceCode)
    {
        ResetValues();

        _sourceCode = sourceCode;

        while (EndOfCode() == false)
        {
            _startIndex = _index;
            char character = GetNextCharacter();
            IToken token;
            switch (character)
            {
                case '(':
                    token = new TokenOperator() { Type = TokenType.OpeningParenthesis, Value = character.ToString() };
                    AddToken(token);
                    continue;
                case ')':
                    token = new TokenOperator() { Type = TokenType.ClosingParenthesis, Value = character.ToString() };
                    AddToken(token);
                    continue;
                case '"':
                    GetStringLiteral();
                    continue;
                case '[':
                    token = new TokenOperator() { Type = TokenType.OpeningSquareBracket, Value = character.ToString() };
                    AddToken(token);
                    GetCharactersLiterals();
                    continue;
                case ']':
                    token = new TokenOperator() { Type = TokenType.ClosingSquareBracket, Value = character.ToString() };
                    AddToken(token);
                    continue;
                case '{':
                    token = new TokenOperator() { Type = TokenType.OpeningCurlyBracket, Value = character.ToString() };
                    AddToken(token);
                    continue;
                case '}':
                    token = new TokenOperator() { Type = TokenType.ClosingCurlyBracket, Value = character.ToString() };
                    AddToken(token);
                    continue;
                case var isWhitespace when new Regex("\\s").IsMatch(isWhitespace.ToString()):
                    continue;
                case var isLetter when new Regex("[a-zA-Z]").IsMatch(isLetter.ToString()):
                    GetName();
                    continue;
                case var isNumber when new Regex("[0-9]").IsMatch(isNumber.ToString()):
                    GetNumberLiteral();
                    continue;
                default:
                    throw new UnexpectedCharaterException($"{character} Is not a valid character");
            }

        }

        return _tokens;
    }

    private static void ResetValues()
    {
        _sourceCode = "";
        _startIndex = 0;
        _index = 0;
        _tokens = new();
    }

    private static void GetNumberLiteral()
    {
        while (char.IsDigit(PeekCharacter()) && EndOfCode() == false)
            GetNextCharacter();
        
        string number = _sourceCode.Substring(_startIndex, _index - _startIndex);
        AddToken(new TokenNumber() { Value = Int32.Parse(number) });
    }

    private static void GetName()
    {
        while (char.IsLetterOrDigit(PeekCharacter()) && EndOfCode() == false)
            GetNextCharacter();
        string name = _sourceCode.Substring(_startIndex, _index - _startIndex);

        AddToken(new TokenKeyword() { Value = name });
    }

    private static void GetCharactersLiterals()
    {
        while (PeekCharacter() is not ']' && Char.IsWhiteSpace(PeekCharacter()) == false && EndOfCode() == false)
        {
            TokenCharacter tokenCharacter = new() { Character = GetNextCharacter() };
            AddToken(tokenCharacter);
        }
 
    }

    private static void GetStringLiteral()
    {
        while (PeekCharacter() is not '"' && EndOfCode() == false)
           GetNextCharacter();

        if (EndOfCode())
        {
            throw new MissingEndOperator("Missing closing \" ");
        }
        GetNextCharacter(); // Get rid of "
        string value = _sourceCode.Substring(_startIndex + 1, _index - _startIndex - 2);
        AddToken(new TokenString() { Value = value });
    }

    private static bool EndOfCode()
    {
        return _index >= _sourceCode.Length;
    }
    private static char GetNextCharacter()
    {
        return _sourceCode[_index++];
    }
    private static char PeekCharacter()
    {
        if (EndOfCode())
            return '\0';
        return _sourceCode[_index];
    }

    private static void AddToken(IToken token)
    {
        _tokens.Add(token);

    }

}
