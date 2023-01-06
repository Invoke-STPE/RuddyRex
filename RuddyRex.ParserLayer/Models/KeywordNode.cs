using RuddyRex.Core.Interfaces.NodeInterface;
using RuddyRex.Core.Interfaces.NodeInterfaces;
using RuddyRex.Core.Interfaces.RegexInterface;
using RuddyRex.Core.Interfaces.VisitorInterfaces;
using RuddyRex.Core.Types;

namespace RuddyRex.ParserLayer.Models;

public record KeywordNode : IStringValueNode
{
    public NodeType Type => NodeType.Keyword;
    public string Value { get; set; }

    public IRegexNode Accept(IConvorterVisitor visitor)
    {
        return visitor.ConvertKeyword(this);
    }
}
