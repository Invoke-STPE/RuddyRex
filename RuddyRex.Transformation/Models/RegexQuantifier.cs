using RuddyRex.ParserLayer;
using RuddyRex.ParserLayer.Interfaces;

namespace RuddyRex.Transformation.Models;

public record RegexQuantifier : IRegexNode
{
    public RegexType Type { get; set; } = RegexType.Quantifier;
    public string Kind { get; set; }
    public int From { get; set; }
    public int To { get; set; }

    internal bool IsAsteriskRange()
    {
        return To == 0 && From == 0;
    }

    internal bool IsPlusRange()
    {
        return To == 1 && From == 1 || Kind == "+";
    }

    internal bool IsQuestionMarkRange()
    {
        return From == 0 && To == 1;
    }
}
