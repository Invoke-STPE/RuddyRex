using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuddyRex.Lib.Enums
{
    public enum NodeType
    {
        NumberLiteral,
        StringLiteral,
        GroupExpression,
        KeywordExpression,
        None,
        RangeExpression,
        CharacterRange,
        CharacterNode,
        Keyword,
    }
}
