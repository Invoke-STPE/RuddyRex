using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuddyRex.Lib
{
    public static class RuddyRexDictionary
    {
        private static List<string> _astDictionary= new() { "Match"};
        private static List<string> _keyWorddictionary= new() { "Between"};
        private static List<string> _returnValues = new() { "digit", "letter" };
        public static bool IsValidStartKeyword(string keyword)
        {
            return _astDictionary.Contains(keyword);
        }
        public static bool IsValidKeyword(string keyword)
        {
            return _keyWorddictionary.Contains(keyword);
        }
        public static bool IsValidReturnValue(string keyword)
        {
            return _returnValues.Contains(keyword);
        }
    }
}
