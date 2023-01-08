using RuddyRex.Core;
using RuddyRex.Core.Interfaces.RegexInterface;
using RuddyRex.Core.Types;

namespace RuddyRex.CodeGeneration
{
    public static class CodeGenerator
    {
        public static string GenerateCode(AbstractTree<IRegexNode> tree)
        {
            string output = "";
            foreach (var node in tree.Nodes)
            {
                switch (node.Type)
                {
                    case RegexType.Group:
                        IRegexNode regexGroup = node;
                        output += regexGroup.ToString();
                        break;
                    case RegexType.CharacterClass:
                        IRegexNode regexCharacterClass = node;
                        output += regexCharacterClass.ToString();
                        break;
                    case RegexType.Char:
                        IRegexNode regexChar = node;
                        output += regexChar.ToString();
                        break;
                    case RegexType.Repetition:
                        IRegexNode regexRepetition = node;
                        output += regexRepetition.ToString();
                        break;
                    case RegexType.Quantifier:
                        IRegexNode regexQuantifier = node;
                        output += regexQuantifier.ToString();
                        break;
                    case RegexType.Alternative:
                        IRegexNode regexAlternative = node;
                        output += regexAlternative.ToString();
                        break;
                    case RegexType.ClassRange:
                        IRegexNode regexClassRange = node;
                        output += regexClassRange.ToString();
                        break;
                    default:
                        break;
                }
            }
            return output;
        }
    }
}
