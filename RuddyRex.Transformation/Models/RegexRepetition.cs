using RuddyRex.Core.Interfaces.RegexInterface;
using RuddyRex.Core.Types;
namespace RuddyRex.Transformation.Models;
public record RegexRepetition : IRegexNode
{
    public RegexType Type { get; set; } = RegexType.Repetition;
    public IRegexNode Expression { get; set; }
    public RegexQuantifier Quantifier { get; set; }

    public override string ToString()
    {
        return $"{Expression}{Quantifier}";
    }
}
