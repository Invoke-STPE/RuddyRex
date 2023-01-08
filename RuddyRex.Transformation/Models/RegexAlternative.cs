using RuddyRex.Core.Interfaces.RegexInterface;
using RuddyRex.Core.Types;

namespace RuddyRex.Transformation.Models;
public class RegexAlternative : IRegexNode
{
    public RegexType Type { get; set; } = RegexType.Alternative;
    public List<IRegexNode> Expressions { get; set; } = new List<IRegexNode>();

    public override bool Equals(object? obj)
    {
        return obj is RegexAlternative alternative &&
               Type == alternative.Type &&
               Expressions.SequenceEqual(alternative.Expressions);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Type, Expressions);
    }

    public override string ToString()
    {
        return string.Join("", Expressions.Select(n => n.ToString()));
    }
}
