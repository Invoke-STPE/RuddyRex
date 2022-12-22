using RuddyRex.ParserLayer;
using RuddyRex.ParserLayer.Interfaces;

namespace RuddyRex.Transformation.Models;

public class RegexGroup : IRegexNode
{
    public RegexType Type { get; } = RegexType.Group;
    public List<IRegexNode> Expressions { get; set; } = new();

    public override bool Equals(object? obj)
    {
        return obj is RegexGroup group &&
               Type == group.Type &&
               Expressions.SequenceEqual(group.Expressions); ;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Type, Expressions);
    }
}
