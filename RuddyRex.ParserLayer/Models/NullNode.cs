using RuddyRex.ParserLayer.Interfaces;

namespace RuddyRex.ParserLayer.Models;

public class NullNode : INode
{
    public NodeType Type { get; } = NodeType.None;
    // Forkortet

    public IRegexNode Accept(IConvorterVisitor visitor)
    {
        throw new NotImplementedException();
    }
}
