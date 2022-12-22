
namespace RuddyRex.LexerLayer.Models;
public record TokenString : IToken
{
    public TokenType Type { get; } = TokenType.StringLiteral;
    public string Value { get; set; }
}
