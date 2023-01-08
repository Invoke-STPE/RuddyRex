using RuddyRex.Core.Interfaces.RegexInterface;
using RuddyRex.Core.Types;

namespace RuddyRex.Transformation.Models
{
    public record RegexClassRange : IRegexNode
    {
        public RegexType Type => RegexType.ClassRange;
        public RegexChar From { get; set; }
        public RegexChar To { get; set; }

        public override string ToString()
        {
            string output = From.IsLetterRange() ? $"{From}{To}" : $"{From}-{To}";
            return output;
        }
    }
}
