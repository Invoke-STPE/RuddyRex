using RuddyRex.Lib.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuddyRex.Lib.Models.TokenModels
{
    public record TokenCharacter : IToken
    {
        public TokenType Type { get; } = TokenType.CharacterLiteral;
        public char Character { get; set; }
    }
}
