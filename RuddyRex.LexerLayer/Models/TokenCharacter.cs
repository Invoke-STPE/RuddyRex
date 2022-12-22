
namespace RuddyRex.LexerLayer.Models;

public record TokenCharacter : IToken
{
    public TokenType Type { get; } = TokenType.CharacterLiteral;
    public char Character { get; set; }
}
