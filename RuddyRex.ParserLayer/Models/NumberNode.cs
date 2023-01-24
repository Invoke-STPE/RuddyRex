using RuddyRex.Core.Interfaces.NodeInterface;
using RuddyRex.Core.Interfaces.NodeInterfaces;
using RuddyRex.Core.Interfaces.RegexInterface;
using RuddyRex.Core.Interfaces.VisitorInterfaces;
using RuddyRex.Core.Types;

namespace RuddyRex.ParserLayer.Models;

public record NumberNode : INumberNode
{
    public NodeType Type { get; } = NodeType.NumberLiteral;
    public int Value { get; set; }

    public IRegexNode Accept(IConvorterVisitor visitor)
    {
        throw new NotImplementedException();
    }
    public override string ToString()
    {
        return Type.ToString();
    }
}
