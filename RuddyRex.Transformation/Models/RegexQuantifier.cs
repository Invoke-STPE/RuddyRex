using RuddyRex.ParserLayer;
using RuddyRex.ParserLayer.Interfaces;

namespace RuddyRex.Transformation.Models;

public record RegexQuantifier : IRegexNode
{
    public RegexType Type { get; set; }
    public string Kind { get; set; }
    public int From { get; set; }
    public int To { get; set; }
}
