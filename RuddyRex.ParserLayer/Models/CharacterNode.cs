using RuddyRex.Core.Interfaces.NodeInterface;
using RuddyRex.Core.Interfaces.NodeInterfaces;
using RuddyRex.Core.Interfaces.RegexInterface;
using RuddyRex.Core.Interfaces.VisitorInterfaces;
using RuddyRex.Core.Types;

namespace RuddyRex.ParserLayer.Models;

public record CharacterNode : ICharValueNode
{
    public NodeType Type { get; } = NodeType.CharacterNode;
    public char Value { get; set; }


    public IRegexNode Accept(IConvorterVisitor visitor)
    {
        return visitor.ConvertChar(this);
    }
}
