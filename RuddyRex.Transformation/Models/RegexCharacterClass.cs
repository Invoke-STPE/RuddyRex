using RuddyRex.ParserLayer;
using RuddyRex.ParserLayer.Interfaces;

namespace RuddyRex.Transformation.Models;

public class RegexCharacterClass : IRegexNode
{
    public RegexType Type { get; set; }
    public List<IRegexNode> Expressions { get; set; } = new List<IRegexNode>();

    public override bool Equals(object? obj)
    {
        return obj is RegexCharacterClass @class &&
               Type == @class.Type &&
               Expressions.SequenceEqual(@class.Expressions);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Type, Expressions);
    }
}
