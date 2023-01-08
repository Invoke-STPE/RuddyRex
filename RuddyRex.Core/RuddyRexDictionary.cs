namespace RuddyRex.Core
{
    public static class RuddyRexDictionary
    {
        private static List<string> _astDictionary= new() { "Match"};
        private static List<string> _keyWorddictionary= new() { "Between", "Exactly"};
        private static List<string> _returnValues = new() { "digit", "letter", "digits", "letters" };
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
