using RuddyRex.ParserLayer;
using RuddyRex.ParserLayer.Interfaces;

namespace RuddyRex.Transformation.Models;
public record RegexRepetition : IRegexNode
{
    public RegexType Type { get; set; }
    public IRegexNode Expression { get; set; }
    public RegexQuantifier Quantifier { get; set; }
}
