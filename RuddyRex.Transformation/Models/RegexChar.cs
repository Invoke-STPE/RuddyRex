using RuddyRex.ParserLayer;
using RuddyRex.ParserLayer.Interfaces;

namespace RuddyRex.Transformation.Models;

public record RegexChar : IRegexNode
{
    public RegexType Type { get;} = RegexType.Char;
    public string Value { get; set; }
    public string Kind { get; set; } = "simple";
    public char Symbol { get; set; }

    public override string ToString()
    {
        return Value;
    }
    public bool IsLetterRange()
    {
        return Value == "a-z" || Value == "A-Z";
    }
}
