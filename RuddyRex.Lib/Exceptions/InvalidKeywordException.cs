using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuddyRex.Lib.Exceptions
{
    public class InvalidKeywordException : Exception
    {
        public InvalidKeywordException(string? message) : base(message)
        {
        }
    }
}
