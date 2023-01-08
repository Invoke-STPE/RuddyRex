
using RuddyRex.Core.Interfaces.TokenInterfaces;
using RuddyRex.Core.Types;
namespace RuddyRex.LexerLayer.Models;
public record TokenString : ITokenString
{
    public TokenType Type { get; } = TokenType.StringLiteral;
    public string Value { get; set; }
}
