using RuddyRex.Core.Interfaces.RegexInterface;
using RuddyRex.Core.Interfaces.VisitorInterfaces;
using RuddyRex.Core.Types;

namespace RuddyRex.Core.Interfaces.NodeInterface;

public interface INode
{
    public NodeType Type { get; }
    IRegexNode Accept(IConvorterVisitor visitor);
}
