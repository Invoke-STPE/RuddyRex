﻿using RuddyRex.Core.Interfaces.NodeInterface;
using RuddyRex.Core.Interfaces.NodeInterfaces;
using RuddyRex.Core.Interfaces.RegexInterface;
using RuddyRex.Core.Interfaces.VisitorInterfaces;
using RuddyRex.Core.Types;

namespace RuddyRex.ParserLayer.Models;

public record KeywordNode : IStringValueNode
{
    public NodeType Type => NodeType.Keyword;
    public string Value { get; set; } = "";

    public IRegexNode Accept(IConvorterVisitor visitor)
    {
        return visitor.ConvertKeyword(this);
    }

    public override string ToString()
    {
        return Type.ToString();
    }

    //public override bool Equals(object? obj)
    //{
    //    return obj is KeywordNode node &&
    //           Type == node.Type &&
    //           Value == node.Value;
    //}

    //public override int GetHashCode()
    //{
    //    return HashCode.Combine(Type, Value);
    //}
}
