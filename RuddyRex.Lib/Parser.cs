using RuddyRex.Lib.Enums;
using RuddyRex.Lib.Exceptions.SyntaxExceptions;
using RuddyRex.Lib.Models;
using RuddyRex.Lib.Models.Interfaces;
using RuddyRex.Lib.Models.NodeModels;
using RuddyRex.Lib.Models.RuddyRex.NodeModels;
using RuddyRex.Lib.Models.RuddyRex.TokenModels;
using RuddyRex.Lib.Models.TokenModels;


namespace RuddyRex.Lib
{
    public class Parser
    {
        private static Queue<IToken> _tokens;
        static Stack<IToken> brackets;

        public static AbstractTree<INode> ParseTree(List<IToken> tokens)
        {
            _tokens = new Queue<IToken>(tokens);
            brackets = new Stack<IToken>();
            AbstractTree<INode> tree = CreateAST(NextToken());
            while (PeekToken().Type is not TokenType.None)
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
            return token.Type switch
            {
                TokenType.OpeningParenthesis => AnalyseOpeningParenthesis(token),
                TokenType.ClosingParenthesis => AnalyseClosingParenthesis(),
                TokenType.OpeningSquareBracket => AnalyseOpeningSquareBracket(token),
                TokenType.ClosingSquareBracket => AnalyseClosingSqaureBracket(),
                TokenType.OpeningCurlyBracket => AnalyseOpeningCurlyBracket(token),
                TokenType.ClosingCurlyBracket => AnalyseClosingCurlyBracket(),
                TokenType.CharacterLiteral => AnalyseCharacterLiteral(token),
                TokenType.KeywordIdentifier => AnalyseKeywordIdentifier(token),
                TokenType.NumberLiteral => AnalyseNumberLiteral(token),
                TokenType.AlternateOperator => new NullNode(),
                TokenType.StringLiteral => new NullNode(),
                TokenType.None => new NullNode(),
            };
        }

        private static INode AnalyseCharacterLiteral(IToken token)
        {
            TokenCharacter tokenCharacter = (TokenCharacter)token;
            return new CharacterNode() { Value = tokenCharacter.Character };
        }

        private static INode AnalyseNumberLiteral(IToken token)
        {
            TokenNumber tokenNumber = (TokenNumber)token;
            return new NumberNode() { Value = tokenNumber.Value };
        }

        private static INode AnalyseKeywordIdentifier(IToken token)
        {
            TokenKeyword tokenKeyword = (TokenKeyword)token;
            if (tokenKeyword.Value.ToLower() == "till")
            {
                return CreateKeywordTill();
            }
            if (RuddyRexDictionary.IsValidKeyword(tokenKeyword.Value))
            {
                return CreateKeywordExpression(token);
            }
            if (RuddyRexDictionary.IsValidReturnValue(tokenKeyword.Value))
            {
                return CreateValueType(token);
            }
            
            return tokenKeyword.Value.Length == 0 ? throw new InvalidRangeExpression("Invalid range") : throw new InvalidKeywordException("Keyword not regonized");
        }

        private static INode CreateKeywordTill()
        {
            return new KeywordNode() { Value = "till" };
        }
        private static INode CreateKeywordExpression(IToken token)
        {
            TokenKeyword tokenKeyword = (TokenKeyword)token;
    
            KeywordExpressionNode keywordNode = new KeywordExpressionNode()
            {
                Keyword = tokenKeyword.Value,
                Parameter = AnalyseToken(NextToken()),
                ValueType = (KeywordNode)AnalyseToken(NextToken())
            };

            return keywordNode;
        }

        private static INode CreateValueType(IToken token)
        {
            TokenKeyword tokenKeyword = (TokenKeyword)token;
           
            return new KeywordNode() { Value = tokenKeyword.Value };
           
        }
        private static INode AnalyseClosingCurlyBracket()
        {
            if (brackets.Count == 0 || brackets.Pop().Type != TokenType.OpeningCurlyBracket) 
                throw new InvalidRangeExpression("Missing bracket");

            return AnalyseToken(NextToken());
        }

        private static INode AnalyseOpeningCurlyBracket(IToken token)
        {
            brackets.Push(token);
            RangeNode rangeNode = new RangeNode();
            KeywordNode keyword = null;
            while (PeekToken()?.Type != TokenType.ClosingCurlyBracket)
            {
                if (PeekToken()?.Type == TokenType.NumberLiteral)
                {
                    rangeNode.Values.Add(AnalyseToken(NextToken()));
                }
                else if (PeekToken()?.Type == TokenType.KeywordIdentifier)
                {
                    keyword = (KeywordNode)AnalyseToken(NextToken());
                    if (keyword.Value.ToLower() != "till")
                    {
                        throw new InvalidRangeExpression("Invalid keyword in range expression");
                    }
                }
            }
            if (rangeNode.Values.Count == 2)
            {
                if (keyword?.Value.ToLower() != "till")
                {
                    throw new InvalidRangeExpression("Invalid keyword in range expression");
                }
            }
            if (rangeNode.Values.Count == 0) 
                throw new InvalidRangeExpression("Range expression cannot contain 0 numbers");
            return rangeNode;
        }

        private static INode AnalyseClosingSqaureBracket()
        {
            if (brackets.Count == 0 || brackets.Pop().Type != TokenType.OpeningSquareBracket)
                throw new InvalidRangeExpression("Missing bracket");
            return new NullNode();
        }

        private static INode AnalyseOpeningSquareBracket(IToken token)
        {
            brackets.Push(token);
            CharacterRangeNode characterRangeNode = new CharacterRangeNode();
            while (PeekToken()?.Type == TokenType.CharacterLiteral)
            {
                characterRangeNode.Characters.Add(AnalyseToken(NextToken()));
            }
            return characterRangeNode;
        }

        private static INode AnalyseClosingParenthesis()
        {
            if (brackets.Count == 0 || brackets.Pop().Type != TokenType.OpeningParenthesis)
                throw new InvalidRangeExpression("Missing bracket");
            return new NullNode();
        }

        private static INode AnalyseOpeningParenthesis(IToken token)
        {
            brackets.Push(token);
            GroupNode groupNode = new GroupNode();

            INode analysedNode = AnalyseToken(NextToken());
            if (analysedNode.Type != NodeType.None)
            {
                groupNode.Nodes.Add(analysedNode);
            }
            return groupNode;
        }

        // https://refactoring.guru/introduce-null-object
        static IToken? NextToken()
        {
            if (_tokens.TryDequeue(out IToken result))
            {
                return result;
            }
            return new TokenNull();
        }
        private static IToken? PeekToken()
        {
            return _tokens.TryPeek(out IToken result) ? result : new TokenNull();
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

