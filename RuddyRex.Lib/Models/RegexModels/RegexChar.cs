using RuddyRex.Lib.Enums;
using RuddyRex.Lib.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuddyRex.Lib.Models.RegexModels
{
    public record RegexChar : IRegexNode
    {
        public RegexType Type { get; set; }
        public string Value { get; set; }
        public string Kind { get; set; } = "simple";
        public char Symbol { get; set; }
    }
}
