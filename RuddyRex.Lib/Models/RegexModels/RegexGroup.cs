using RuddyRex.Lib.Enums;
using RuddyRex.Lib.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuddyRex.Lib.Models.RegexModels
{
    public record RegexGroup : IRegexNode
    {
        public RegexType Type { get; set; }
        public List<IRegexNode> Expressions { get; set; } = new();
    }
}
