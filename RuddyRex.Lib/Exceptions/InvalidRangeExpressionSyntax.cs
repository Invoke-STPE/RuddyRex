using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuddyRex.Lib.Exceptions
{
    internal class InvalidRangeExpressionSyntax : Exception
    {
        public InvalidRangeExpressionSyntax(string? message) : base(message)
        {
        }
    }
}
