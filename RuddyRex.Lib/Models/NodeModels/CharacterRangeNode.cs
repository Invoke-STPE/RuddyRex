using RuddyRex.Lib.Enums;
using RuddyRex.Lib.Models.TokenModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuddyRex.Lib.Models.NodeModels
{
    public class CharacterRangeNode : INode
    {
        public NodeType Type { get; set; }
        public List<INode> Characters { get; set; } = new();

    }
}
