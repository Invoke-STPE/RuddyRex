using RuddyRex.Lib.Enums;
using RuddyRex.Lib.Exceptions.SyntaxExceptions;
using RuddyRex.Lib.Models;
using RuddyRex.Lib.Models.Interfaces;
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

        public static AbstractTree<INode> Parse(List<IToken> tokens)
        {
            bool firstTime = true;
            _tokenQueue = new Queue<IToken>(tokens);
            AbstractTree<INode> ast = CreateAST(NextToken());
             _token = NextToken();

            while (_tokenQueue.Count() != 0 || firstTime)
            {
                firstTime = false;
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
                        keywordNode.Parameter = ParseRangeExpression();
                        keywordToken = (TokenKeyword)NextToken();
                        if (!RuddyRexDictionary.IsValidReturnValue(keywordNode.ValueType) && keywordToken.Type != TokenType.KeywordIdentifier)
                            throw new InvalidValueType($"{keywordNode.ValueType} is not a valid type");
                        keywordNode.ValueType = keywordToken.Value;
                        node = keywordNode;
                        _token = _token.Type == TokenType.ClosingCurlyBracket ? NextToken() : _token;
                    }
                    else
                    {
                        throw new InvalidValueType($"{keywordToken.Value} is not a valid type"); // TODO: Unit test misspelled return values 
                    }
                    break;
                case TokenType.OpeningSquareBracket: // Need to be incoorporate in other unit tests.
                    CharacterRangeNode characterRange = new() { Type = NodeType.CharacterRange };
                    while (PeekToken().Type != TokenType.ClosingSquareBracket)
                    {
                        TokenCharacter tokenCharacter = (TokenCharacter)NextToken();
                         CharacterNode characterNode = new CharacterNode() { Type = NodeType.CharacterNode, Value = tokenCharacter.Character};
                        characterRange.Characters.Add(characterNode);
                    }
                    if (PeekToken().Type == TokenType.ClosingSquareBracket)
                    {
                        NextToken();
                    } else { throw new UnbalancedBracketsException("Missing closing ']' character"); } // TODO: Skal unit testes
                    node = characterRange;
                    break;
                case TokenType.StringLiteral:
                    StringNode stringNode = new() { Type = NodeType.StringLiteral };
                    TokenString tokenString = (TokenString)_token; 
                    stringNode.Value = tokenString.Value;
                    node= stringNode;
                    break;
            }
            return node;
        }

        private static RangeNode ParseRangeExpression()
        {
            _token = NextToken();
            if (_token.Type != TokenType.OpeningCurlyBracket) throw new InvalidRangeExpression("Syntax Error: Invalid range syntax.");
            Stack<IToken> stack = new Stack<IToken>();
            stack.Push(_token);
            RangeNode rangeNode = new RangeNode() { Type = NodeType.RangeExpression };
            while(stack.Count > 0)
            {
                _token = NextToken();
                switch (_token.Type)
                {
                    case TokenType.ClosingCurlyBracket:
                        if (stack.Count == 0 || stack.Pop().Type != TokenType.OpeningCurlyBracket) throw new InvalidRangeExpression($"Missing a closing }}");
                        break;
                    case TokenType.NumberLiteral:
                        TokenNumber tokenNumber = (TokenNumber)_token;
                        rangeNode.Values.Add(tokenNumber);
                        break;
                    case TokenType.KeywordIdentifier:
                        TokenKeyword identifier = (TokenKeyword)_token;
                        if (identifier.Value != "Till" ) throw new InvalidRangeExpression("A range expression can only contain the keyword 'Till'");
                        break;
                    default:
                        throw new InvalidRangeExpression("Unknown character in range expression.");
                }
            }
            //NextToken();
            return stack.Count == 0 ? rangeNode : throw new InvalidRangeExpression("Unable to parse range syntax."); ;
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
                        if (stack.Count == 0 || stack.Pop().Type != TokenType.OpeningParenthesis) throw new UnbalancedBracketsException("Uneven pair of parenthesis!");
                        break;
                    case TokenType.KeywordIdentifier:
                        KeywordNode keywordNode = (KeywordNode)AnalyseToken(_token);
                        node.Nodes.Add(keywordNode);
                        continue;
                    case TokenType.OpeningSquareBracket:
                        node.Nodes.Add(AnalyseToken(_token));
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
        private static IToken PeekToken()
        {
            return _tokenQueue.TryPeek(out IToken result) ? result : null;
        }

        private static AbstractTree<INode> CreateAST(IToken? token)
        {
            TokenKeyword keyword = (TokenKeyword)token;
            if (RuddyRexDictionary.IsValidStartKeyword((keyword.Value)))
            {
                return new AbstractTree<INode>() { Type = keyword.Value };
            }
            throw new InvalidKeywordException($"{keyword.Value} Is not a valid keyword Identifier");
        }
    }
}
