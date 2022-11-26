using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuddyRex.Lib.Enums
{
    public enum TokenType
    {
        // Operator definitions
        OpeningParenthesis,
        ClosingParenthesis,
        OpeningSquareBracket,
        ClosingSquareBracket,
        OpeningCurlyBracket,
        ClosingCurlyBracket,
        AlternateOperator,
        // Literal and keyword definitions
        KeywordIdentifier,
        NumberLiteral,
        CharacterLiteral,
        StringLiteral,
    }
}
