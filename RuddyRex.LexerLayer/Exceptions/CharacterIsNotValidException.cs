namespace RuddyRex.LexerLayer.Exceptions;

public class CharacterIsNotValidException : Exception
{
    public CharacterIsNotValidException(string? message) : base(message)
    {
    }
}
