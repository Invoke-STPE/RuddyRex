using RuddyRex.Lib.Enums;
using RuddyRex.Lib.Models.TokenModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuddyRex.Lib.Models.RuddyRex.TokenModels
{
    public class TokenNull : IToken
    {
        public TokenType Type => TokenType.None;
    }
}
