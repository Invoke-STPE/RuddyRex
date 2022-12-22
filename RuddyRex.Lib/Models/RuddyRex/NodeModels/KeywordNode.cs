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
    public record KeywordNode : INode
    {
        public NodeType Type => NodeType.Keyword;
        public string Value { get; set; }

        public IRegexNode Accept(IConvorterVisitor visitor)
        {
            throw new NotImplementedException();
        }
    }
}
