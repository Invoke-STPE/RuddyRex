using RuddyRex.Lib.Enums;
using RuddyRex.Lib.Models.Interfaces;
using RuddyRex.Lib.Models.TokenModels;
using RuddyRex.Lib.Visitor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuddyRex.Lib.Models.NodeModels
{
    public class RangeNode : INode
    {
        public NodeType Type { get; set; }
        public List<IToken> Values { get; set; } = new List<IToken>();

        public override bool Equals(object? obj)
        {
            return obj is RangeNode node &&
                   Type == node.Type;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Type);
        }

        public IRegexNode OnEnter(IVisitor visitor)
        {
            return visitor.ConvertRange(this);
        }
    }
}
