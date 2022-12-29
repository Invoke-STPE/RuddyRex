using RuddyRex.ParserLayer.Interfaces;

namespace RuddyRex.ParserLayer.Models;

public record NumberNode : INode
{
    public NodeType Type { get; } = NodeType.NumberLiteral;
    public int Value { get; set; }

    public IRegexNode Accept(IConvorterVisitor visitor)
    {
        throw new NotImplementedException();
    }
}
