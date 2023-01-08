using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuddyRex.Core.Interfaces.TokenInterfaces
{
    public interface ITokenInt : IToken
    {
        public int Value { get; set; }
    }
}
