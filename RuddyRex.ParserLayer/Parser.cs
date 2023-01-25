using RuddyRex.Core;
using RuddyRex.Core.Exceptions;
using RuddyRex.Core.Interfaces.NodeInterface;
using RuddyRex.Core.Interfaces.TokenInterfaces;
using RuddyRex.Core.Types;
using RuddyRex.ParserLayer.DTO;
using RuddyRex.ParserLayer.Models;
using RuddyRex.ParserLayer.ErrorHandling;

namespace RuddyRex.ParserLayer;

public static class Parser
{
    private static Queue<IToken> _tokens;
    static Stack<IToken> brackets;
    private static IToken currentToken = null;

    public static AbstractTree<INode> ParseTree(List<IToken> tokens)
    {
        _tokens = new Queue<IToken>(tokens);
        brackets = new Stack<IToken>();
        AbstractTree<INode> tree = CreateAST(NextToken());
        while (PeekToken().Type is not TokenType.None)
        {
            IToken _currentToken = NextToken();

            try
            {
                INode node = AnalyseToken(_currentToken);
                if (node.Type is not NodeType.None)
                {
                    tree.Nodes.Add(node);
                }
            }
            catch (Exception ex)
            {
                RuddyRexError.Report(ex);
            }
            

        }
        if (brackets.Count > 0) RuddyRexError.Report(new ExpectedBracketException("Expected bracket pair, but found none."));
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
            TokenType.StringLiteral => AnalyseStringLiteral(token),
            TokenType.None => new NullNode(),
        };
    }

    private static INode AnalyseStringLiteral(IToken token)
    {
        ITokenString tokenString = (ITokenString)token;
        return new StringNode() { Value = tokenString.Value };
    }

    private static INode AnalyseCharacterLiteral(IToken token)
    {
        ITokenChar tokenCharacter = (ITokenChar)token;
        return new CharacterNode() { Value = tokenCharacter.Character };
    }

    private static INode AnalyseNumberLiteral(IToken token)
    {
        ITokenInt tokenNumber = (ITokenInt)token;
        return new NumberNode() { Value = tokenNumber.Value };
    }

    private static INode AnalyseKeywordIdentifier(IToken token)
    {
        ITokenString tokenKeyword = (ITokenString)token;
        try
        {
            if (tokenKeyword.Value.ToLower() == "space")
            {
                return CreateKeyword(tokenKeyword.Value);
            }
            if (tokenKeyword.Value.ToLower() == "any")
            {
                return CreateKeyword(tokenKeyword.Value);
            }
            if (tokenKeyword.Value.ToLower() == "alternate")
            {
                return CreateKeyword(tokenKeyword.Value);
            }
            if (tokenKeyword.Value.ToLower() == "till")
            {
                return CreateKeyword(tokenKeyword.Value);
            }
            if (RuddyRexDictionary.IsValidKeyword(tokenKeyword.Value))
            {
                return CreateKeywordExpression(token);
            }
            if (RuddyRexDictionary.IsValidReturnValue(tokenKeyword.Value))
            {
                return CreateValueType(token);
            }
            throw new InvalidKeywordException($"Invalid keyword: {tokenKeyword.Value} is not regonized as a keyword");
        }
        catch (Exception)
        {
            
            Synchronize();
            throw;
        }
        
    }
    private static INode CreateKeyword(string value)
    {
        return new KeywordNode() { Value = value };
    }
    private static INode CreateKeywordExpression(IToken token)
    {
        ITokenString tokenKeyword = (ITokenString)token;
        KeywordExpressionNode keywordNode = new KeywordExpressionNode()
        {
            Keyword = tokenKeyword.Value,
            Parameter = AnalyseToken(NextToken()),

            ValueType = PeekToken().Type != TokenType.None ? (KeywordNode)AnalyseToken(NextToken()) : throw new InvalidKeywordException("Invalid keyword between range expression")
        };

        return keywordNode;
    }

    private static void Synchronize()
    {
        Console.WriteLine($"Error found at token {currentToken.Type}, starting synchronization");
        Console.WriteLine("Skipping tokens untill synchronization point is found...");
        while (_tokens.Count != 0)
        {
            switch (PeekToken().Type)
            {
                case TokenType.None:
                case TokenType.OpeningParenthesis:
                case TokenType.OpeningSquareBracket:
                case TokenType.ClosingSquareBracket:
                case TokenType.StringLiteral:
                    Console.WriteLine($"Found synchronization point at: {currentToken.Type}");
                    return;
                case TokenType.KeywordIdentifier:
                    ITokenString tokenString = (ITokenString)PeekToken();
                    if (RuddyRexDictionary.IsValidStartKeyword(tokenString.Value))
                    {
                        Console.WriteLine($"Found synchronization point at: {currentToken.Type}");
                        return;
                    }
                    break;
            }
            NextToken();
            Console.WriteLine($"{currentToken.Type} is not a synchronization point");
        }
    }

    private static INode CreateValueType(IToken token)
    {
        ITokenString tokenKeyword = (ITokenString)token;
       
        return new KeywordNode() { Value = tokenKeyword.Value };
       
    }
    private static INode AnalyseClosingCurlyBracket()
    {
        if (brackets.Count == 0 || brackets.Pop().Type != TokenType.OpeningCurlyBracket) 
            throw new ExpectedBracketException("Expected closing '}', but found none.");

        if (PeekToken().Type == TokenType.ClosingParenthesis)
            return new NullNode();
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
                rangeNode.Nodes.Add(AnalyseToken(NextToken()));
            }
            else if (PeekToken()?.Type == TokenType.KeywordIdentifier)
            {
                keyword = (KeywordNode)AnalyseToken(NextToken());
                if (keyword.Value.ToLower() != "till")
                {
                    throw new InvalidRangeExpression($"Invalid keyword {keyword.Value} is not regonized as a valid keyword");
                }
            }
        }
        if (rangeNode.Nodes.Count == 2)
        {
            if (keyword?.Value.ToLower() != "till")
            {
                throw new InvalidRangeExpression($"Invalid keyword {keyword.Value} is not regonized as a valid keyword");
            }
        }
        if (rangeNode.Nodes.Count == 0) 
            throw new InvalidRangeExpression("Range expression cannot contain 0 numbers");
        return rangeNode;
    }

    private static INode AnalyseClosingSqaureBracket()
    {
        if (brackets.Count == 0 || brackets.Pop().Type != TokenType.OpeningSquareBracket)
            throw new ExpectedBracketException("Expected bracket pair, but found none.");
        return new NullNode();
    }

    private static INode AnalyseOpeningSquareBracket(IToken token)
    {
        brackets.Push(token);
        CharacterRangeNode characterRangeNode = new CharacterRangeNode();
        while (PeekToken()?.Type == TokenType.CharacterLiteral)
        {
            characterRangeNode.Nodes.Add(AnalyseToken(NextToken()));
        }
        return characterRangeNode;
    }

    private static INode AnalyseClosingParenthesis()
    {
        if (brackets.Count == 0 || brackets.Pop().Type != TokenType.OpeningParenthesis)
            throw new ExpectedBracketException("Expected bracket pair, but found none.");
        return new NullNode();
    }

    private static INode AnalyseOpeningParenthesis(IToken token)
    {
        brackets.Push(token);
        GroupNode groupNode = new GroupNode();

        while (PeekToken().Type != TokenType.ClosingParenthesis && _tokens.Count != 0)
        {
            INode analysedNode = AnalyseToken(NextToken());
            if (analysedNode.Type != NodeType.None)
            {
                groupNode.Nodes.Add(analysedNode);
            }  
        }
        return groupNode;
    }

    static IToken? NextToken()
    {
        if (_tokens.TryDequeue(out IToken result))
        {
            currentToken = result;
            return result;
        }
        return new NullValueToken();
    }
    private static IToken? PeekToken()
    {
        return _tokens.TryPeek(out IToken result) ? result : new NullValueToken();
    }

    private static AbstractTree<INode> CreateAST(IToken? token)
    {
        ITokenString keyword = (ITokenString)token;
        if (RuddyRexDictionary.IsValidBeginKeyword((keyword.Value)))
        {
            return new AbstractTree<INode>() { Type = keyword.Value };
        }
        throw new InvalidKeywordException($"{keyword.Value} is not a valid start keyword, try Match");
    }
}

