using RuddyRex.ParserLayer.Interfaces;

namespace RuddyRex.ParserLayer.Models;

public record CharacterNode : INode
{
    public NodeType Type { get; } = NodeType.CharacterNode;
    public char Value { get; set; }


    public IRegexNode Accept(IConvorterVisitor visitor)
    {
        return visitor.ConvertToChar(this);
    }
}
