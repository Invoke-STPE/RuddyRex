using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuddyRex.Transformation.Exceptions
{
    public class InvalidSemanticException : Exception
    {
        public InvalidSemanticException(string? message) : base(message)
        {
        }
    }
}
