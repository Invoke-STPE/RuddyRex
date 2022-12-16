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
        public NodeType Type { get; } = NodeType.RangeExpression;
        public List<INode> Values { get; set; } = new List<INode>();

        public override bool Equals(object? obj)
        {
            return obj is RangeNode node &&
                   Type == node.Type &&
                   Values.SequenceEqual(node.Values);
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
