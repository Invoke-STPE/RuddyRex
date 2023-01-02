using RuddyRex.ParserLayer;
using RuddyRex.ParserLayer.Interfaces;
using RuddyRex.ParserLayer.Models;
using RuddyRex.Transformation.Models;

namespace RuddyRex.Transformation
{
    public static class CodeOptimizer
    {
        public static AbstractTree<IRegexNode> OptimizeTree(AbstractTree<IRegexNode> tree)
        {
            AbstractTree<IRegexNode> optimizedTree = new AbstractTree<IRegexNode>() { Type = "RegExp" };
            foreach (var node in tree.Nodes)
            {
                var optimizedNode = OptimizeNode(node);
                optimizedTree.Nodes.Add(optimizedNode);
                if (optimizedNode.Type != RegexType.Quantifier)
                    continue;

                OptimizeCharacterLiteral(optimizedTree.Nodes);
            }
            return optimizedTree;
        }

        private static IRegexNode OptimizeNode(IRegexNode node)
        {
            IRegexNode output = null;
                switch (node.Type)
                {
                    case RegexType.CharacterClass:
                        RegexCharacterClass characterClass = (RegexCharacterClass)node;
                        output = ValidateCharacterClassNode(characterClass);
                        break;
                    case RegexType.Char:
                        output = node;
                        break;
                    case RegexType.Repetition:
                        RegexRepetition repetition = (RegexRepetition)node;
                        repetition.Expression = OptimizeNode(repetition.Expression);
                        output = repetition;
                        break;
                    case RegexType.Quantifier:
                        output = node;
                        break;
                    case RegexType.Alternative:
                        RegexAlternative alternative = (RegexAlternative)node;
                        alternative.Expressions = alternative.Expressions.Select(n => OptimizeNode(n)).ToList();
                        output = alternative;
                        break;
                    case RegexType.ClassRange:
                        output = node;
                        break;
                    case RegexType.Group:
                        output = ValidateGroupNode((RegexGroup)node);
                        break;
                    default:
                        break;
                }
                return output;
        }

        private static IRegexNode? ValidateCharacterClassNode(RegexCharacterClass characterClass)
        {
            if (characterClass.Expressions.Count == 1 && characterClass.Expressions.First().Type != RegexType.ClassRange)
            {
                return characterClass.Expressions.First();
            }

            return characterClass;
        }

        private static IRegexNode ValidateGroupNode(RegexGroup node)
        {
            if (node.Expressions.Count == 1)
            {
                IRegexNode firstNode = node.Expressions.First();
                if (firstNode.Type != RegexType.Repetition && firstNode.Type != RegexType.Alternative)
                    return node.Expressions.First();
            }

            OptimizeCharacterLiteral(node.Expressions);
            return node;
        }

        private static void OptimizeCharacterLiteral(List<IRegexNode> nodes)
        {
            List<IRegexNode> continuouslyMetChars = new List<IRegexNode>();
            for (int i = 0; i < nodes.Count - 1; i++)
            {
                if (nodes[i].Type != RegexType.Char)
                    continue;
                continuouslyMetChars.Add(nodes[i]);
                if (nodes[i + 1].Type != RegexType.Quantifier)
                    continue;

                if (continuouslyMetChars.Count == 1)
                {
                    RegexQuantifier quantifier = (RegexQuantifier)nodes[i + 1];
                    if (quantifier.IsPlusRange())
                    {
                        RegexChar regexChar = (RegexChar)nodes[i];
                        RegexChar copyOfChar = new RegexChar() { Kind = regexChar.Kind, Symbol = regexChar.Symbol, Value = regexChar.Value };
                        quantifier.Kind = "*";
                        nodes.Insert(i + 1, copyOfChar);
                    } 
                }
                else
                {
                    continuouslyMetChars.Clear();
                }
            }
        }
    }
}
