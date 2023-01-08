using RuddyRex.Core.Interfaces.TokenInterfaces;
using RuddyRex.Core.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuddyRex.ParserLayer.DTO
{
    public class NullValueToken : IToken
    {
        public TokenType Type => TokenType.None;
    }
}
