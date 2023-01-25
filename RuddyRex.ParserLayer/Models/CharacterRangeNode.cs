using RuddyRex.Core.Interfaces.NodeInterface;
using RuddyRex.Core.Interfaces.NodeInterfaces;
using RuddyRex.Core.Interfaces.RegexInterface;
using RuddyRex.Core.Interfaces.VisitorInterfaces;
using RuddyRex.Core.Types;

namespace RuddyRex.ParserLayer.Models;

public class CharacterRangeNode : IParentNode
{
    public NodeType Type { get; } = NodeType.CharacterRange;
    public List<INode> Nodes { get; set; } = new();


    public override bool Equals(object? obj)
    {
        return obj is CharacterRangeNode node &&
               Type == node.Type &&
               Nodes.SequenceEqual(node.Nodes); ;
    }

    public IRegexNode Accept(IConvorterVisitor visitor)
    {
        return visitor.ConvertCharacterClass(this);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Type, Nodes);
    }

    public override string ToString()
    {
        return Type.ToString();
    }
}
