using RuddyRex.Core.Interfaces.NodeInterface;
using RuddyRex.Core.Interfaces.RegexInterface;
using RuddyRex.Core.Interfaces.VisitorInterfaces;
using RuddyRex.Core.Types;

namespace RuddyRex.ParserLayer.Models;

public class NullNode : INode
{
    public NodeType Type { get; } = NodeType.None;

    public IRegexNode Accept(IConvorterVisitor visitor)
    {
        throw new NotImplementedException();
    }
}
