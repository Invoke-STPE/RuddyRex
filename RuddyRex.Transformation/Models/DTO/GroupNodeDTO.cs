using RuddyRex.Core.Interfaces.NodeInterface;
using RuddyRex.Core.Interfaces.NodeInterfaces;
using RuddyRex.Core.Interfaces.RegexInterface;
using RuddyRex.Core.Interfaces.VisitorInterfaces;
using RuddyRex.Core.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuddyRex.Transformation.Models.DTO
{
    public class GroupNodeDTO : IParentNode
    {
        public List<INode> Nodes { get; set; } = new();

        public NodeType Type => NodeType.GroupExpression;

        public IRegexNode Accept(IConvorterVisitor visitor)
        {
            return visitor.ConvertToGroup(this);
        }
    }
}
