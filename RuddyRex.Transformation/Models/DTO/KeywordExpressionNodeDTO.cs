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
    public class KeywordExpressionNodeDTO : IExpressionNode
    {
        public string Keyword { get; set; } = "";
        public INode Parameter { get; set; }
        public IStringValueNode ValueType { get; set; } = new KeywordNodeDTO();

        public NodeType Type => NodeType.KeywordExpression;

        public IRegexNode Accept(IConvorterVisitor visitor)
        {
            throw new NotImplementedException();
        }

        public bool IsExactlyKeyword()
        {
            return Keyword.ToLower() == "exactly";
        }
    }
}
