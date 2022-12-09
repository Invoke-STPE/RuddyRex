﻿using RuddyRex.Lib.Enums;
using RuddyRex.Lib.Models.Interfaces;
using RuddyRex.Lib.Visitor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuddyRex.Lib.Models.NodeModels
{
    public class KeywordNode : INode
    {
        public NodeType Type { get; set; }
        public string Keyword { get; set; }
        public INode Parameter { get; set; }
        public string ValueType { get; set; } = "";

        public override bool Equals(object? obj)
        {
            return obj is KeywordNode node &&
                   Type == node.Type &&
                   Keyword == node.Keyword &&
                   ValueType == node.ValueType;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Type, Keyword, ValueType);
        }

        public IRegexNode OnEnter(IVisitor visitor)
        {
            return visitor.ConvertKeyword(this);
        }
    }
}