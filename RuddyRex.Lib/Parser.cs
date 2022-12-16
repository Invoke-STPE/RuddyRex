using RuddyRex.Lib.Enums;
using RuddyRex.Lib.Exceptions.SyntaxExceptions;
using RuddyRex.Lib.Models;
using RuddyRex.Lib.Models.Interfaces;
using RuddyRex.Lib.Models.NodeModels;
using RuddyRex.Lib.Models.RuddyRex.NodeModels;
using RuddyRex.Lib.Models.TokenModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RuddyRex.Lib
{
    /// <summary>
    /// Refactor IDEA, https://stackoverflow.com/questions/7377344/how-to-write-a-parser-in-c
    /// </summary>
    public class Parser
    {
        private static Queue<IToken> _tokens;
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
                if (node.Type is not NodeType.None)
                {
                    tree.Nodes.Add(node); 
                }

            }
            if (brackets.Count > 0) throw new InvalidRangeExpression("Test");
            return tree;
        }

        private static INode AnalyseToken(IToken token)
        {
        // Possible refactoring ? https://refactoring.guru/smells/switch-statements
        // https://refactoring.guru/replace-conditional-with-polymorphism
            INode node = new NullNode();
            switch (token.Type)
            {
                case TokenType.OpeningParenthesis:
                    brackets.Push(token);
                    GroupNode groupNode = new GroupNode();
                    INode analysedNode = AnalyseToken(NextToken());
                    if (analysedNode.Type != NodeType.None)
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
                    CharacterRangeNode characterRangeNode = new CharacterRangeNode();
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
                    brackets.Push(token);
                    RangeNode rangeNode = new RangeNode();
                    KeywordNode tillKeyword = null;
                    while (PeekToken()?.Type != TokenType.ClosingCurlyBracket)
                    {
                        if (PeekToken()?.Type == TokenType.NumberLiteral)
                        {
                            rangeNode.Values.Add(AnalyseToken(NextToken())); 
                        }
                        else if (PeekToken()?.Type == TokenType.KeywordIdentifier)
                        {
                            tillKeyword = (KeywordNode)AnalyseToken(NextToken());
                            if (tillKeyword.Keyword.ToLower() != "till")
                            {
                                throw new InvalidRangeExpression("Invalid keyword in range expression");
                            }
                        }
                    }
                    if (rangeNode.Values.Count == 2)
                    {
                        if (tillKeyword?.Keyword.ToLower() != "till")
                        {
                            throw new InvalidRangeExpression("Invalid keyword in range expression");
                        }
                    }
                    if (rangeNode.Values.Count == 0) throw new InvalidRangeExpression("Range expression cannot contain 0 numbers");
                    node = rangeNode;
                    break;
                case TokenType.ClosingCurlyBracket:
                    if (brackets.Count == 0 || brackets.Pop().Type != TokenType.OpeningCurlyBracket) throw new InvalidRangeExpression("Missing bracket");
                    break;
                case TokenType.AlternateOperator:
                    break;
                case TokenType.KeywordIdentifier:
                    TokenKeyword tokenKeyword = (TokenKeyword)token;
                    if (tokenKeyword.Value.ToLower() == "till")
                    {
                        node = new KeywordNode() { Keyword = tokenKeyword.Value };
                        break;
                    }
                    if(RuddyRexDictionary.IsValidKeyword(tokenKeyword.Value))
                    {
                        KeywordNode keywordNode = new KeywordNode()
                        {
                            Keyword = tokenKeyword.Value,
                            Parameter = AnalyseToken(NextToken()),
                        };
                        if (PeekToken().Type == TokenType.ClosingCurlyBracket)
                        {
                            AnalyseToken(NextToken());
                        }
                        // I NEED A BETTER WAY THAN THIS
                        KeywordNode keywordNode2 = (KeywordNode)AnalyseToken(NextToken());
                        keywordNode.ValueType = keywordNode2.Keyword;
                        node = keywordNode;
                        break;
                    }

                    if (RuddyRexDictionary.IsValidReturnValue(tokenKeyword.Value))
                    {
                        node = new KeywordNode() { Keyword = tokenKeyword.Value };
                    }
                    break;
                case TokenType.NumberLiteral:
                    TokenNumber tokenNumber = (TokenNumber)token;
                    node = new NumberNode() {  Value = tokenNumber.Value };
                    break;
                case TokenType.CharacterLiteral:
                    TokenCharacter tokenCharacter = (TokenCharacter)token;
                    node = new CharacterNode() { Value = tokenCharacter.Character };
                    break;
                case TokenType.StringLiteral:
                    break;
                default:
                    break;
            }
            return node;
        }
        // https://refactoring.guru/introduce-null-object
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

