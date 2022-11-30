using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuddyRex.Lib.Exceptions
{
    public class UnbalancedBracketsException : Exception
    {
        public UnbalancedBracketsException(string? message) : base(message)
        {
        }
    }
}
