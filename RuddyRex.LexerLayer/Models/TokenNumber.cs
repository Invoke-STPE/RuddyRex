
namespace RuddyRex.LexerLayer.Models;

public record TokenNumber : IToken
{
    public TokenType Type { get; } = TokenType.NumberLiteral;
    public int Value { get; set; }
}
