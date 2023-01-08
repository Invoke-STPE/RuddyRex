namespace RuddyRex.Core.Types;

public enum TokenType
{
    None,
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
}
