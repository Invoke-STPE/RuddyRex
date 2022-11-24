using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuddyRex.Lib.Exceptions
{
    public class KeywordWasNotRecognizedException : Exception
    {
        public KeywordWasNotRecognizedException(string? message) : base(message)
        {
        }
    }
}
