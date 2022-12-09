using RuddyRex.Lib.Enums;
using RuddyRex.Lib.Models.Interfaces;
using RuddyRex.Lib.Visitor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuddyRex.Lib.Models.NodeModels
{
    public record StringNode : INode
    {
        public NodeType Type { get; set; }
        public string Value { get; set; }

        public IRegexNode OnEnter(IVisitor visitor)
        {
            return visitor.ConvertString(this);
        }
    }
}
