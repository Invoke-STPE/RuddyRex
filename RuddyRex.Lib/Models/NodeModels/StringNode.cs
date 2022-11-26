using RuddyRex.Lib.Enums;
using RuddyRex.Lib.Models.TokenModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuddyRex.Lib.Models.NodeModels
{
    public record StringNode : INode
    {
        public NodeTypes Type { get; set; }
        public string Value { get; set; }
    }
}
