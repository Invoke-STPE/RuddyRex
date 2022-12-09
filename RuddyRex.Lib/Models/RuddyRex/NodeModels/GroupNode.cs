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
    public record GroupNode : INode
    {
        public NodeType Type { get; set; }
        public List<INode> Nodes { get; set; } = new List<INode>();

        //public override bool Equals(object? obj)
        //{
        //    return obj is GroupNode node &&
        //           Type == node.Type;
        //}

        public override int GetHashCode()
        {
            return HashCode.Combine(Type);
        }

        public IRegexNode OnEnter(IVisitor visitor)
        {
            return visitor.ConvertGroup(this);
        }
    }
}
