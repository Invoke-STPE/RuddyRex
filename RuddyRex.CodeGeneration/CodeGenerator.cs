using RuddyRex.ParserLayer;
using RuddyRex.ParserLayer.Interfaces;
using RuddyRex.ParserLayer.Models;
using RuddyRex.Transformation.Models;

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
                        RegexGroup regexGroup = (RegexGroup)node;
                        output += regexGroup.ToString();
                        break;
                    case RegexType.CharacterClass:
                        RegexCharacterClass regexCharacterClass = (RegexCharacterClass)node;
                        output += regexCharacterClass.ToString();
                        break;
                    case RegexType.Char:
                        RegexChar regexChar = (RegexChar)node;
                        output += regexChar.ToString();
                        break;
                    case RegexType.Repetition:
                        RegexRepetition regexRepetition = (RegexRepetition)node;
                        output += regexRepetition.ToString();
                        break;
                    case RegexType.Quantifier:
                        RegexQuantifier regexQuantifier = (RegexQuantifier)node;
                        output += regexQuantifier.ToString();
                        break;
                    case RegexType.Alternative:
                        RegexAlternative regexAlternative = (RegexAlternative)node;
                        output += regexAlternative.ToString();
                        break;
                    case RegexType.ClassRange:
                        RegexClassRange regexClassRange = (RegexClassRange)node;
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
