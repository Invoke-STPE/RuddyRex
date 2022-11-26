using RuddyRex.Lib.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuddyRex.Lib.Models.NodeModels
{
    public record NumberNode : INode
    {
        public NodeTypes Type { get; set; }
        public int Value { get; set; }
    }
}
