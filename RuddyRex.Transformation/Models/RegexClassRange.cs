using RuddyRex.ParserLayer;
using RuddyRex.ParserLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
