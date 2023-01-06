
using RuddyRex.Core.Interfaces.TokenInterfaces;
using RuddyRex.Core.Types;
namespace RuddyRex.LexerLayer.Models;

public record TokenNumber : ITokenInt
{
    public TokenType Type { get; } = TokenType.NumberLiteral;
    public int Value { get; set; }
}
