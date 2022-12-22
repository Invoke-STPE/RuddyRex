using RuddyRex.Lib.Enums;
using RuddyRex.Lib.Models.Interfaces;
using RuddyRex.Lib.Models.TokenModels;
using RuddyRex.Lib.Visitor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuddyRex.Lib.Models.RuddyRex.NodeModels
{
    public record NumberNode : INode
    {
        public NodeType Type { get; } = NodeType.NumberLiteral;
        public int Value { get; set; }

        public IRegexNode Accept(IConvorterVisitor visitor)
        {
            throw new NotImplementedException();
        }
    }
}
