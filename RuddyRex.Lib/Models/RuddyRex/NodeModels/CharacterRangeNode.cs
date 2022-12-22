using RuddyRex.Lib.Enums;
using RuddyRex.Lib.Models.Interfaces;
using RuddyRex.Lib.Models.TokenModels;
using RuddyRex.Lib.Visitor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RuddyRex.Lib.Models.NodeModels
{
    public class CharacterRangeNode : INode
    {
        public NodeType Type { get; } = NodeType.CharacterRange;
        public List<INode> Characters { get; set; } = new();

        public override bool Equals(object? obj)
        {
            return obj is CharacterRangeNode node &&
                   Type == node.Type &&
                   Characters.SequenceEqual(node.Characters); ;
        }

        public IRegexNode Accept(IConvorterVisitor visitor)
        {
            return visitor.ConvertToCharacterClass(this);
        }
    }
}
