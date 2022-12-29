using RuddyRex.ParserLayer;
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
                    From = new RegexChar() { Value = "a", Symbol = 'a'},
                    To = new RegexChar() {Value = "z", Symbol = 'z'},
                },
                new RegexClassRange()
                {
                    From = new RegexChar() { Value = "A", Symbol = 'A'},
                    To = new RegexChar() {Value = "Z", Symbol = 'Z'},
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

    public RegexType Type { get; set; }
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
}
