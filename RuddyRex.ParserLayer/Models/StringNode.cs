using RuddyRex.ParserLayer.Interfaces;

namespace RuddyRex.ParserLayer.Models;

public record StringNode : INode
{
    public NodeType Type { get; } = NodeType.StringLiteral;
    public string Value { get; set; }

    public IRegexNode Accept(IConvorterVisitor visitor)
    {
        return visitor.ConvertString(this);
    }
}
