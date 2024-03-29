﻿using RuddyRex.Core.Interfaces.NodeInterface;
using RuddyRex.Core.Interfaces.NodeInterfaces;
using RuddyRex.Core.Interfaces.RegexInterface;
using RuddyRex.Core.Interfaces.VisitorInterfaces;
using RuddyRex.Core.Types;

namespace RuddyRex.ParserLayer.Models;

public class KeywordExpressionNode : IExpressionNode
{
    public NodeType Type { get; } = NodeType.KeywordExpression;
    public string Keyword { get; set; } = "";
    public INode Parameter { get; set; }
    public IStringValueNode ValueType { get; set; } = new KeywordNode();

    public IRegexNode Accept(IConvorterVisitor visitor)
    {
        return visitor.ConvertKeywordExpression(this);
    }

    public override bool Equals(object? obj)
    {
        return obj is KeywordExpressionNode node &&
               Type == node.Type &&
               Keyword == node.Keyword &&
               EqualityComparer<INode>.Default.Equals(Parameter, node.Parameter) &&
               EqualityComparer<IStringValueNode>.Default.Equals(ValueType, node.ValueType);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Type, Keyword, Parameter, ValueType);
    }

    public bool IsExactlyKeyword()
    {
        return Keyword.ToLower() == "exactly";
    }

    public override string ToString()
    {
        return Type.ToString();
    }
}
