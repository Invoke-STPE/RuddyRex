using RuddyRex.Lib.Enums;
using RuddyRex.Lib.Models;
using RuddyRex.Lib.Models.NodeModels;
using RuddyRex.Lib.Models.TokenModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace RuddyRex.Lib
{
    public class Parser
    {
        public static AbstractTree ParseAST(List<IToken> tokens)
        {
            AbstractTree ast = new AbstractTree();
            int index = 0;

            while (index <= tokens.Count)
            {
                var (node, newIndex) = Walk(index, tokens);
                ast.Nodes.Add(node);
                index = newIndex;
            }
            return ast;
        }

        private static (INode, int) Walk(int index, List<IToken> tokens)
        {
            IToken token = tokens[index];
            INode node = null;

            switch (token.Type)
            {
                case Enums.TokenType.Name:

                    break;
                case Enums.TokenType.Operator:
                    break;
                case Enums.TokenType.NumerLiteral:
                    index++;
                    TokenNumber tokenNumber = (TokenNumber)token;
                    node = new NumberNode() { Type = NodeTypes.NumberLiteral, Value = tokenNumber.Value };
                    break;
                case Enums.TokenType.CharacterLiteral:
                    break;
                case Enums.TokenType.StringLiteral:
                    index++;
                    TokenString tokenString = (TokenString)token;
                    node = new StringNode() { Type = NodeTypes.StringLiteral, Value = tokenString.Value };
                    break;
                default:
                    break;
            }
            return (node, 3);
            
        }
    }
}
