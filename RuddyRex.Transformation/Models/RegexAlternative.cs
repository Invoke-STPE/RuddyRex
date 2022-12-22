using RuddyRex.ParserLayer;
using RuddyRex.ParserLayer.Interfaces;

namespace RuddyRex.Transformation.Models;
public class RegexAlternative : IRegexNode
{
    public RegexType Type { get; set; }
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
}
