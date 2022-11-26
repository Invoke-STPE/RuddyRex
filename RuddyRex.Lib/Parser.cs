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
                case TokenType.OpeningParenthesis:
                    break;
                case TokenType.ClosingParenthesis:
                    break;
                case TokenType.OpeningSquareBracket:
                    break;
                case TokenType.ClosingSquareBracket:
                    break;
                case TokenType.OpeningCurlyBracket:
                    break;
                case TokenType.ClosingCurlyBracket:
                    break;
                case TokenType.AlternateOperator:
                    break;
                case TokenType.KeywordIdentifier:
                    break;
                case TokenType.NumberLiteral:
                    break;
                case TokenType.CharacterLiteral:
                    break;
                case TokenType.StringLiteral:
                    break;
            }
            return (node, 3);

        }
    }
}
