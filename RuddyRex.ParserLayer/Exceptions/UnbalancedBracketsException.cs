namespace RuddyRex.ParserLayer.Exceptions;
public class UnbalancedBracketsException : Exception
    {
        public UnbalancedBracketsException(string? message) : base(message)
        {
        }
    }
