using RuddyRex.Lib.Enums;
using RuddyRex.Lib.Exceptions.SyntaxExceptions;
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
using System.Xml.Linq;

namespace RuddyRex.Lib
{
    public class Parser
    {
       
        static private Queue<IToken> _tokenQueue;
        static private IToken _token;

        public static AbstractTree Parse(List<IToken> tokens)
        {
            
            _tokenQueue = new Queue<IToken>(tokens);
            AbstractTree ast = CreateAST(NextToken());
            IToken _token = NextToken();

            while (_tokenQueue.Count() != 0)
            {
                var node = AnalyseToken(_token);
                ast.Nodes.Add(node);
            }
            return ast;
        }

        private static INode AnalyseToken(IToken token)
        {
            INode node = null;

            switch (token.Type)
            {
                case TokenType.OpeningParenthesis:
                    GroupNode group = (GroupNode)ParseGroupExpression(token);
                    node = group;
                    break;
                case TokenType.KeywordIdentifier:
                    TokenKeyword keywordToken = (TokenKeyword)token;
                    if (RuddyRexDictionary.IsValidKeyword(keywordToken.Value))
                    {
                        KeywordNode keywordNode = new KeywordNode() { Keyword = keywordToken.Value, Type = NodeType.KeywordExpression };
                        keywordNode.Parameters.Add(ParseRangeExpression());
                        keywordToken = (TokenKeyword)NextToken();
                        if (!RuddyRexDictionary.IsValidReturnValue(keywordNode.ValueType) && keywordToken.Type != TokenType.KeywordIdentifier)
                            throw new InvalidValueType($"{keywordNode.ValueType} is not a valid type");
                        keywordNode.ValueType = keywordToken.Value;
                        node = keywordNode;
                    }
                    else
                    {
                        throw new InvalidValueType($"{keywordToken.Value} is not a valid type"); // Test misspelled digit
                    }
                    break;
            }
            return node;
        }

        private static RangeNode ParseRangeExpression()
        {
            _token = NextToken();
            if (_token.Type != TokenType.OpeningCurlyBracket) throw new InvalidRangeExpressionSyntax("Syntax Error: Invalid range syntax.");
            Stack<IToken> stack = new Stack<IToken>();
            stack.Push(_token);
            RangeNode rangeNode = new RangeNode() { Type = NodeType.RangeExpression };
            while(stack.Count > 0)
            {
                _token = NextToken();
                switch (_token.Type)
                {
                    case TokenType.ClosingCurlyBracket:
                        if (stack.Count == 0 || stack.Pop().Type != TokenType.OpeningCurlyBracket) throw new InvalidRangeExpressionSyntax($"Missing a closing }}");
                        break;
                    case TokenType.NumberLiteral:
                        TokenNumber tokenNumber = (TokenNumber)_token;
                        rangeNode.Values.Add(tokenNumber);
                        break;
                    case TokenType.KeywordIdentifier:
                        TokenKeyword identifier = (TokenKeyword)_token;
                        if (identifier.Value != "Till" ) throw new InvalidRangeExpressionSyntax("A range expression can only contain the keyword 'Till'"); // Need to unit test
                        break;
                    default:
                        throw new InvalidRangeExpressionSyntax("Unknown character in range expression."); // Needs an unit test cannot have ( in a range exprtession
                }
            }
            return stack.Count == 0 ? rangeNode : throw new InvalidRangeExpressionSyntax("Unable to parse range syntax."); ;
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
                    case TokenType.OpeningParenthesis: // Implement multiple groups
                        stack.Push(_token);
                        node.Nodes.Add(new GroupNode() { Type = NodeType.GroupExpression});
                        break;
                    case TokenType.ClosingParenthesis:
                        if (stack.Count == 0 || stack.Pop().Type != TokenType.OpeningParenthesis) throw new UnbalancedBracketsException("Uneven pair of parenthesis!");
                        break;
                    case TokenType.KeywordIdentifier:
                        KeywordNode keywordNode = (KeywordNode)AnalyseToken(_token);
                        node.Nodes.Add(keywordNode);
                        break;
                    default:
                        break;
                }
                if (_tokenQueue.Count == 0) break; // If token queue is empty, no reason to try and fetch the next one. 
                _token = NextToken();
               
            }
            return stack.Count == 0 ? node : throw new UnbalancedBracketsException("Uneven pair of parenthesis!");
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
