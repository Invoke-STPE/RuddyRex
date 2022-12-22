
namespace RuddyRex.LexerLayer.Models;

public record TokenKeyword : IToken
{
    public TokenType Type { get; } = TokenType.KeywordIdentifier;
    public string Value { get; set; }
}
