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
                if (character.IsLetter())
                {
                    string letters = character.ToString();
                    //character = input[++index];
                    while (input[++index].IsLetter())
                    {
                        letters += input[index];
                        if (index == input.Length - 1)
                        {
                            break;
                        }
                    }

                    tokens.Add(new TokenName() { Type = TokenType.Name, Value = letters });
                    continue;
                }
                if (character.IsNumber())
                {
                    // Opgave går opmærksom på } og hvordan du debuggede
                    string number = character.ToString();
                    //character = input[++index];
                    while (input[++index].IsNumber())
                    {
                        number += input[index];
                    }
                    tokens.Add(new TokenNumber() { Type = TokenType.Number, Value = Int32.Parse(number) });
                    continue;
                }
                index++;
            }

            return tokens;
        }
    }
}
