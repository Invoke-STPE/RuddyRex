namespace RuddyRex.Core
{
    public static class RuddyRexDictionary
    {
        private static List<string> _astDictionary= new() { "match"};
        private static List<string> _keyWorddictionary= new() { "between", "exactly"};
        private static List<string> _returnValues = new() { "digit", "letter", "digits", "letters" };
        public static bool IsValidStartKeyword(string keyword)
        {
            return _astDictionary.Contains(keyword.ToLower());
        }
        public static bool IsValidKeyword(string keyword)
        {
            return _keyWorddictionary.Contains(keyword.ToLower());
        }
        public static bool IsValidReturnValue(string keyword)
        {
            return _returnValues.Contains(keyword.ToLower());
        }
    }
}
