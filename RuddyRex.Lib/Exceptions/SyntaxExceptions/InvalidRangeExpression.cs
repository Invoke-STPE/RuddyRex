using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuddyRex.Lib.Exceptions.SyntaxExceptions
{
    public class InvalidRangeExpression : Exception
    {
        public InvalidRangeExpression(string? message) : base(message)
        {
        }
    }
}
