using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuddyRex.Lib.Helpers
{
    public static class KeywordDictionary
    {
        private static List<string> keywords = new List<string>()
        {
            "Between",
            "Exactly",
            "Match",
            "Substitute",
            "Start",
            "End",
            "Till",
            "Digit",
            "Letter"
        };

        public static bool IsValidKeyword(string keyword)
        {
            return keywords.Contains(keyword);
        }
    }
}
