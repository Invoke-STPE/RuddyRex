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
                        IRegexNode regexGroup = (IRegexNode)node;
                        output += regexGroup.ToString();
                        break;
                    case RegexType.CharacterClass:
                        IRegexNode regexCharacterClass = (IRegexNode)node;
                        output += regexCharacterClass.ToString();
                        break;
                    case RegexType.Char:
                        IRegexNode regexChar = (IRegexNode)node;
                        output += regexChar.ToString();
                        break;
                    case RegexType.Repetition:
                        IRegexNode regexRepetition = (IRegexNode)node;
                        output += regexRepetition.ToString();
                        break;
                    case RegexType.Quantifier:
                        IRegexNode regexQuantifier = (IRegexNode)node;
                        output += regexQuantifier.ToString();
                        break;
                    case RegexType.Alternative:
                        IRegexNode regexAlternative = (IRegexNode)node;
                        output += regexAlternative.ToString();
                        break;
                    case RegexType.ClassRange:
                        IRegexNode regexClassRange = (IRegexNode)node;
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
