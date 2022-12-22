namespace RuddyRex.LexerLayer;

public enum TokenType
{
    // Operator definitions
    OpeningParenthesis,
    ClosingParenthesis,
    OpeningSquareBracket,
    ClosingSquareBracket,
    OpeningCurlyBracket,
    ClosingCurlyBracket,
    AlternateOperator,
    // Literal and keyword definitions
    KeywordIdentifier,
    NumberLiteral,
    CharacterLiteral,
    StringLiteral,
    None,
}
