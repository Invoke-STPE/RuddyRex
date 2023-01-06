using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuddyRex.Core.Interfaces.TokenInterfaces
{
    public interface ITokenChar : IToken
    {
        public char Character { get; set; }
    }
}
