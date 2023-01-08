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
    public class KeywordNodeDTO : IStringValueNode
    {
        public string Value { get; set; }

        public NodeType Type => NodeType.StringLiteral;

        public IRegexNode Accept(IConvorterVisitor visitor)
        {
            return visitor.ConvertString(this);
        }
    }
}
