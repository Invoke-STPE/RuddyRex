using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuddyRex.Core.Interfaces.TokenInterfaces
{
    public interface ITokenString : IToken
    {
        public string Value { get; set; }
    }
}
