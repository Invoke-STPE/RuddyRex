using RuddyRex.Core.Interfaces.NodeInterface;
using RuddyRex.Core.Interfaces.NodeInterfaces;
using RuddyRex.Core.Interfaces.RegexInterface;
using RuddyRex.Core.Interfaces.VisitorInterfaces;
using RuddyRex.Core.Types;

namespace RuddyRex.ParserLayer.Models;

public record StringNode : IStringValueNode
{
    public NodeType Type { get; } = NodeType.StringLiteral;
    public string Value { get; set; }

    public IRegexNode Accept(IConvorterVisitor visitor)
    {
        return visitor.ConvertString(this);
    }
}
