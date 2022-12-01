using RuddyRex.Lib.Enums;
using RuddyRex.Lib.Exceptions;
using RuddyRex.Lib.Models;
using RuddyRex.Lib.Models.NodeModels;
using RuddyRex.Lib.Models.TokenModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
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
            AbstractTree ast = CreateAST(NextToken());
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
                case TokenType.KeywordIdentifier:
                    TokenKeyword keyword = (TokenKeyword)token;
                    if (RuddyRexDictionary.IsValidKeyword(keyword.Value))
                    {
                        node = ParseExpression(keyword);

                    }
                    break;
            }
            return node;
        }

        private static KeywordNode ParseExpression(TokenKeyword keyword)
        {
            KeywordNode node = new KeywordNode() { Keyword = keyword.Value, Type = NodeType.KeywordExpression };
            IToken token = NextToken();
            if (token.Type == TokenType.OpeningCurlyBracket)
            {
                RangeNode rangeNode = ParseRangeExpression();
                node.Parameters.Add(rangeNode);
                if (NextToken().Type != TokenType.ClosingCurlyBracket) throw new UnbalancedBracketsException("You're missing a '}' in your range expression."); // TODO: Test missing curly
                token = NextToken();
                if (token.Type == TokenType.KeywordIdentifier)
                {
                    TokenKeyword tokenKeyword = (TokenKeyword)token;
                    if (RuddyRexDictionary.IsValidReturnValue(tokenKeyword.Value))
                    {
                        node.ValueType = tokenKeyword.Value;
                    } else
                    {
                        throw new InvalidValueType($"{tokenKeyword.Value} is not a valid type"); // Test misspelled digit
                    }
                }
            }
            else
            {
                throw new InvalidRangeExpressionSyntax("A curly bracket is missing from your expression");
            }
            return node;
        }

        private static RangeNode ParseRangeExpression()
        {
            MANGLER FEJL HÅNDTERING 
            RangeNode rangeNode = new RangeNode() { Type = NodeType.RangeExpression };
            for (int i = 0; i < 3; i++)
            {
                IToken token = NextToken();
                switch (token.Type)
                {
                    case TokenType.NumberLiteral:
                        TokenNumber tokenNumber = (TokenNumber)token;
                        rangeNode.Values.Add(tokenNumber);
                        break;
                    case TokenType.KeywordIdentifier:
                        TokenKeyword identifier = (TokenKeyword)token;
                        if (identifier.Value != "Till" ) throw new InvalidKeywordException("A range expression can only contain the keyword 'Till'"); // Need to unit test
                        break;
                    default:
                        throw new InvalidRangeExpressionSyntax("Range Expression contains invalid syntax.");
                }
            }
            return rangeNode;
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


        private static AbstractTree CreateAST(IToken? token)
        {
            TokenKeyword keyword = (TokenKeyword)token;
            if (RuddyRexDictionary.IsValidStartKeyword((keyword.Value)))
            {
                return new AbstractTree() { Type = keyword.Value };
            }
            throw new InvalidKeywordException($"{keyword.Value} Is not a valid keyword Identifier");
        }
    }
}
