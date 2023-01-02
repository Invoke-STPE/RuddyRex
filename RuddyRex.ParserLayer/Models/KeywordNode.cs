using RuddyRex.ParserLayer.Interfaces;

namespace RuddyRex.ParserLayer.Models;

public record KeywordNode : INode
{
    public NodeType Type => NodeType.Keyword;
    public string Value { get; set; }

    public IRegexNode Accept(IConvorterVisitor visitor)
    {
        return visitor.ConvertKeyword(this);
    }
}
