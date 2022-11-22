using RuddyRex.Lib.Enums;
using RuddyRex.Lib.Extensions;
using RuddyRex.Lib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuddyRex.Lib
{
    public static class Lexer
    {

        public static List<IToken> Tokenize(string input)
        {
            
            List<IToken> tokens = new();
            int index = 0;
            while (index <= input.Length - 1)
            {
                char character = input[index];

                if (character.IsBracket())
                {
                    TokenSymbol symbol = new() { Type = TokenType.Symbol, Value = character.ToString()};
                    tokens.Add(symbol);
                    index++;
                    continue;
                }
            }

            return tokens;
        }
    }
}
