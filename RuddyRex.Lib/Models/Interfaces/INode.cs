using RuddyRex.Lib.Enums;
using RuddyRex.Lib.Visitor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuddyRex.Lib.Models.Interfaces
{
    public interface INode
    {
        public NodeType Type { get; }
        IRegexNode OnEnter(IVisitor visitor);
    }
}
