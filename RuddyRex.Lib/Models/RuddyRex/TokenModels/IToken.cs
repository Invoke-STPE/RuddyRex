using RuddyRex.Lib.Enums;

namespace RuddyRex.Lib.Models.TokenModels
{
    public interface IToken
    {
        public TokenType Type { get; }
    }
}
