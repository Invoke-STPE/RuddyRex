﻿using RuddyRex.ParserLayer;
using RuddyRex.ParserLayer.Interfaces;
using RuddyRex.ParserLayer.Models;

namespace RuddyRex.Transformation.Models;

public class RegexCharacterClass : IRegexNode
{
    public RegexCharacterClass()
    {

    }
    public RegexCharacterClass(KeywordExpressionNode keywordNode)
    {
        if (keywordNode.ValueType.Value == "letter")
        {
            Expressions = new List<IRegexNode>()
            {
                new RegexClassRange()
                {
                    From = new RegexChar() { Value = "a-z", Symbol = 'a'},
                    To = new RegexChar() {Value = "A-Z", Symbol = 'z'},
                }
            };
        }
        if (keywordNode.ValueType.Value == "digit")
        {
            Expressions = new List<IRegexNode>()
            {
                new RegexClassRange()
                {
                    From = new RegexChar() { Value = "1", Symbol = '1'},
                    To = new RegexChar() {Value = "9", Symbol = '9'},
                }
            };
        }
    }

    public RegexType Type { get; set; } = RegexType.CharacterClass;
    public List<IRegexNode> Expressions { get; set; } = new List<IRegexNode>();

    public override bool Equals(object? obj)
    {
        return obj is RegexCharacterClass @class &&
               Type == @class.Type &&
               Expressions.SequenceEqual(@class.Expressions);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Type, Expressions);
    }
    public override string ToString()
    {
        List<string> convertedList = Expressions.Select(n => (RegexChar)n)
                                                .Select(n => n.ToString())
                                                .ToList();
        string output = string.Join("", convertedList);
        return $"[{output}]";
    }
}