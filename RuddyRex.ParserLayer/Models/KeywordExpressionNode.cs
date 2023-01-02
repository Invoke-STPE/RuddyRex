using RuddyRex.ParserLayer.Interfaces;

namespace RuddyRex.ParserLayer.Models;

public class KeywordExpressionNode : INode
{
    public NodeType Type { get; } = NodeType.KeywordExpression;
    public string Keyword { get; set; } = "";
    public INode Parameter { get; set; }
    public KeywordNode ValueType { get; set; } = new();

    public override bool Equals(object? obj)
    {
        return obj is KeywordExpressionNode node &&
               Type == node.Type &&
               Keyword == node.Keyword &&
               ValueType == node.ValueType;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Type, Keyword, ValueType);
    }

    public IRegexNode Accept(IConvorterVisitor visitor)
    {
        return visitor.ConvertKeywordExpression(this);
    }

    public bool IsExactlyKeyword()
    {
        return Keyword.ToLower() == "exactly";
    }
}
