
namespace RuddyRex.LexerLayer.Models;

public record TokenOperator : IToken
{
    public TokenType Type { get; init; } 
    public string Value { get; set; }
}
