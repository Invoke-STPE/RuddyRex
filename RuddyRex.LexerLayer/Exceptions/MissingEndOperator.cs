using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuddyRex.LexerLayer.Exceptions
{
    public class MissingEndOperator : Exception
    {
        public MissingEndOperator(string? message) : base(message)
        {
        }
    }
}
