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
        private static Queue<IToken> _tokens;
        //private static IToken _currentToken = null;
        static Stack<IToken> brackets;

        public static AbstractTree<INode> ParseTree(List<IToken> tokens)
        {
            _tokens = new Queue<IToken>(tokens);
            brackets = new Stack<IToken>();
            AbstractTree<INode> tree = CreateAST(NextToken());
            while (PeekToken() is not null)
            {
                IToken _currentToken = NextToken();
                INode node = AnalyseToken(_currentToken);
                if (node is not null)
                {
                    tree.Nodes.Add(node); 
                }

            }
            if (brackets.Count > 0) throw new InvalidRangeExpression("Test");
            return tree;
        }

        private static INode AnalyseToken(IToken token)
        {
            INode node = null;
            switch (token?.Type)
            {
                case TokenType.OpeningParenthesis:
                    brackets.Push(token);
                    GroupNode groupNode = new GroupNode() { Type = NodeType.GroupExpression };
                    INode analysedNode = AnalyseToken(NextToken());
                    if (analysedNode != null)
                    {
                        groupNode.Nodes.Add(analysedNode);
                    }
                    node = groupNode;
                    break;
                case TokenType.ClosingParenthesis:
                    if (brackets.Count == 0 || brackets.Pop().Type != TokenType.OpeningParenthesis) throw new InvalidRangeExpression("Missing bracket");
                    break;
                case TokenType.OpeningSquareBracket:
                    brackets.Push(token);
                    CharacterRangeNode characterRangeNode = new CharacterRangeNode() { Type = NodeType.CharacterRange };
                    while (PeekToken()?.Type == TokenType.CharacterLiteral)
                    {
                        characterRangeNode.Characters.Add(AnalyseToken(NextToken()));
                    }
                    node = characterRangeNode;
                    break;
                case TokenType.ClosingSquareBracket:
                    if (brackets.Count == 0 || brackets.Pop().Type != TokenType.OpeningSquareBracket) throw new InvalidRangeExpression("Missing bracket");
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
                    TokenCharacter tokenCharacter = (TokenCharacter)token;
                    node = new CharacterNode() { Type = NodeType.CharacterNode, Value = tokenCharacter.Character };
                    break;
                case TokenType.StringLiteral:
                    break;
                default:
                    break;
            }
            return node;
        }

        static IToken? NextToken()
        {
            if (_tokens.TryDequeue(out IToken result))
            {
                return result;
            }
            return null;
        }
        private static IToken? PeekToken()
        {
            return _tokens.TryPeek(out IToken result) ? result : null;
        }

        private static AbstractTree<INode> CreateAST(IToken? token)
        {
            try
            {
                TokenKeyword keyword = (TokenKeyword)token;
                if (RuddyRexDictionary.IsValidStartKeyword((keyword.Value)))
                {
                    return new AbstractTree<INode>() { Type = keyword.Value };
                }
                throw new InvalidKeywordException($"{keyword.Value} Is not a valid keyword Identifier");
            }
            catch (InvalidCastException ex)
            {
                throw ex;
            }
        }
    }
}
