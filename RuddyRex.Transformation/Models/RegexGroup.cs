using RuddyRex.Core.Interfaces.RegexInterface;
using RuddyRex.Core.Types;


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

    public override string ToString()
    {
        string output = string.Join("", Expressions.Select(n => n.ToString()));
        return $"({output})";
    }
}
