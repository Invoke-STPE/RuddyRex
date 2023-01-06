
using RuddyRex.Core.Interfaces.TokenInterfaces;
using RuddyRex.Core.Types;
namespace RuddyRex.LexerLayer.Models;

public record TokenKeyword : ITokenString
{
    public TokenType Type { get; } = TokenType.KeywordIdentifier;
    public string Value { get; set; }
}
