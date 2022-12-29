using RuddyRex.ParserLayer.Interfaces;
namespace RuddyRex.ParserLayer.Models;

public class GroupNode : INode
{
    public NodeType Type { get; } = NodeType.GroupExpression;
    public List<INode> Nodes { get; set; } = new List<INode>();

    public override bool Equals(object? obj)
    {
        return obj is GroupNode node &&
               Type == node.Type &&
               Nodes.SequenceEqual(node.Nodes);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Type);
    }

    public IRegexNode Accept(IConvorterVisitor visitor)
    {
        return visitor.ConvertToGroup(this);
    }
}
