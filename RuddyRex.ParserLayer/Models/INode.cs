using RuddyRex.ParserLayer.Interfaces;

namespace RuddyRex.ParserLayer.Models;

public interface INode
{
    public NodeType Type { get; }
    IRegexNode Accept(IConvorterVisitor visitor);
}
