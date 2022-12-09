using RuddyRex.Lib.Enums;
using RuddyRex.Lib.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuddyRex.Lib.Models.RegexModels
{
    public class RegexQuantifier : IRegexNode
    {
        public RegexType Type { get; set; }
        public string Kind { get; set; }
        public int From { get; set; }
        public int To { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is RegexQuantifier quantifier &&
                   Type == quantifier.Type &&
                   Kind == quantifier.Kind &&
                   From == quantifier.From &&
                   To == quantifier.To;
        }
    }
}
