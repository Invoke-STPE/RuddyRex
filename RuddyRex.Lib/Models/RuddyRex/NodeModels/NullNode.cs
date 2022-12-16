using RuddyRex.Lib.Enums;
using RuddyRex.Lib.Models.Interfaces;
using RuddyRex.Lib.Visitor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuddyRex.Lib.Models.RuddyRex.NodeModels
{
    public class NullNode : INode
    {
        public NodeType Type { get; } = NodeType.None;
        // Forkortet

        public IRegexNode OnEnter(IVisitor visitor)
        {
            throw new NotImplementedException();
        }
    }
}
