
using RuddyRex.Core.Interfaces.TokenInterfaces;
using RuddyRex.Core.Types;
namespace RuddyRex.LexerLayer.Models;

public record TokenOperator : ITokenString
{
    public TokenType Type { get; init; } 
    public string Value { get; set; }
}
