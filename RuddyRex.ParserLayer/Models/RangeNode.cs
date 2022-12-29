using RuddyRex.ParserLayer.Interfaces;

namespace RuddyRex.ParserLayer.Models;

public class RangeNode : INode
{
    public NodeType Type { get; } = NodeType.RangeExpression;
    public List<INode> Values { get; set; } = new List<INode>();

    public override bool Equals(object? obj)
    {
        return obj is RangeNode node &&
               Type == node.Type &&
               Values.SequenceEqual(node.Values);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Type);
    }

    public IRegexNode Accept(IConvorterVisitor visitor)
    {
        return visitor.ConvertRange(this);
    }
}
