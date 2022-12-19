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
            INode output = new NullNode();
            if (tokenKeyword.Value.ToLower() == "till")
            {
                output = CreateKeywordTill();
            }
            if (RuddyRexDictionary.IsValidKeyword(tokenKeyword.Value))
            {
                output = CreateKeywordExpression(token);
            }
            if (RuddyRexDictionary.IsValidReturnValue(tokenKeyword.Value))
            {
                output = CreateValueType(token);
            }
            return output;
        }
        // Change the name of Keyword node, make more sub classes why should Till have a class with empty properties?
        private static INode CreateKeywordTill()
        {
            return new KeywordNode() { Keyword = "till" };
        }
        private static INode CreateKeywordExpression(IToken token)
        {
            TokenKeyword tokenKeyword = (TokenKeyword)token;
    
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
            
            return keywordNode;
        }

        private static INode CreateValueType(IToken token)
        {
            TokenKeyword tokenKeyword = (TokenKeyword)token;
           
            return new KeywordNode() { Keyword = tokenKeyword.Value };
           
        }
        private static INode AnalyseClosingCurlyBracket()
        {
            if (brackets.Count == 0 || brackets.Pop().Type != TokenType.OpeningCurlyBracket) 
                throw new InvalidRangeExpression("Missing bracket");

            return new NullNode();
        }

        private static INode AnalyseOpeningCurlyBracket(IToken token)
        {
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
                    tillKeyword = (KeywordNode)AnalyseToken(NextToken()); // Umuligt at arbejde med <--- WILL HOPEFULLY FIX THIS 
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

