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
        public static bool IsSymbol(this char c)
        {
            return Regex.IsMatch(c.ToString(), "[(){}[\\]|]");
        }
        public static bool IsOpeningSquareBracket(this char c)
        {
            return c.ToString() == "[";
        }
        public static bool IsClosingSquareBracket(this char c)
        {
            return c.ToString() == "]";
        }
        public static bool IsWhiteSpace(this char c)
        {
            return Regex.IsMatch(c.ToString(), "\\s");
        }
        public static bool IsLetter(this char c)
        {
            return Regex.IsMatch(c.ToString(), "[A-Za-z]");
        }
        public static bool IsNumber(this char c)
        {
            return Regex.IsMatch(c.ToString(), "[0-9]");
        }

        public static bool IsQuote(this char c)
        {
            return c.ToString() == "\"";
        }
    }
}
