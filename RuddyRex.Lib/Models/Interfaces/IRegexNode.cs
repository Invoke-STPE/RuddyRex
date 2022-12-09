using RuddyRex.Lib.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuddyRex.Lib.Models.Interfaces
{
    public interface IRegexNode
    {
        public RegexType Type { get; set; }
    }
}
