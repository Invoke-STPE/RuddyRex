using RuddyRex.Lib.Enums;
using RuddyRex.Lib.Exceptions;
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
       
        static private Queue<IToken> _tokenQueue;
        static private IToken _token;


        public static AbstractTree ParseAST(List<IToken> tokens)
        {
            
            _tokenQueue = new Queue<IToken>(tokens);
            AbstractTree ast = new AbstractTree();
            IToken _token = NextToken();

            while (_tokenQueue.Count() != 0)
            {
                var node = Walk(_token);
                ast.Nodes.Add(node);
            }
            return ast;
        }

        private static INode Walk(IToken token)
        {
            INode node = null;

            switch (token.Type)
            {
                case TokenType.OpeningParenthesis:
                    GroupNode group = (GroupNode)ParseGroupExpression(token);
                    node = group;
                    break;

            }
            return node;
        }

        private static INode ParseGroupExpression(IToken token)
        {
            Stack<IToken> stack = new Stack<IToken>();
            stack.Push(token);
            _token = NextToken();
            GroupNode node = new GroupNode() { Type = NodeType.GroupExpression};
            while (stack.Count != 0)
            {
                switch (_token?.Type)
                {
                    case TokenType.OpeningParenthesis:
                        stack.Push(_token);
                        node.Nodes.Add(new GroupNode() { Type = NodeType.GroupExpression});
                        break;
                    case TokenType.ClosingParenthesis:
                        if (stack.Count == 0 || stack.Pop().Type != TokenType.OpeningParenthesis) throw new UnbalancedBracketsException("Uneven pair of brackets!");
                        break;
                    default:
                        break;
                }
                if (_tokenQueue.Count == 0) break; // If token queue is empty, no reason to try and fetch the next one. 
                _token = NextToken();
               
            }
            return stack.Count == 0 ? node : throw new UnbalancedBracketsException("Uneven pair of brackets!");
        }

        static IToken? NextToken()
        {
            if (_tokenQueue.TryDequeue(out IToken result))
            {
                return result;
            }
            return null;
        }
    }
}
