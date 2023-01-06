
using RuddyRex.Core.Interfaces.TokenInterfaces;
using RuddyRex.Core.Types;

namespace RuddyRex.LexerLayer.Models;

public record TokenCharacter : ITokenChar
{
    public TokenType Type { get; } = TokenType.CharacterLiteral;
    public char Character { get; set; }
}
