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
    public record CharacterNode : INode
    {
        public NodeType Type { get; } = NodeType.CharacterNode;
        public char Value { get; set; }


        public IRegexNode OnEnter(IVisitor visitor)
        {
            return visitor.ConvertToChar(this);
        }
    }
}
