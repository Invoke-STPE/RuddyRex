using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RuddyRex.Lib.Extensions
{
    public static class CharExtensions
    {
        /// <summary>
        /// Checks if character is either; ( ) { } [ ] which is valid brackets in RuddyRex
        /// </summary>
        /// <param name="c">Character to be checked</param>
        /// <returns>Bool</returns>
        public static bool IsBracket(this char c)
        {
            return Regex.IsMatch(c.ToString(), "[(){}[\\]]");
        }
    }
}
