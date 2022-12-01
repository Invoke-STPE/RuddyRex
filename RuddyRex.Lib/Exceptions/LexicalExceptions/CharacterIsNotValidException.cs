using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuddyRex.Lib.Exceptions.LexicalExceptions
{
    public class CharacterIsNotValidException : Exception
    {
        public CharacterIsNotValidException(string? message) : base(message)
        {
        }
    }
}
