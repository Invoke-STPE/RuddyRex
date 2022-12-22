using RuddyRex.ParserLayer.Interfaces;
using RuddyRex.ParserLayer.Models;
using RuddyRex.Transformation.Models;
using System.Linq;

namespace RuddyRex.Transformation
{
    public class RegexConvertorVisitor : IConvorterVisitor
    {
        
        public IRegexNode ConvertKeyword(KeywordExpressionNode keywordNode)
        {
            throw new NotImplementedException();
        }

        public IRegexNode ConvertRange(RangeNode rangeNode)
        {
            RegexQuantifier quantifier = new RegexQuantifier();
            if (rangeNode.Values.Count == 1)
            {
                var numbers = rangeNode.Values.Select(x => (NumberNode)x);
                quantifier.From = numbers.Min(x => x.Value);
                quantifier.To = numbers.Min(x => x.Value);
                quantifier.Kind = "Range";
            }
            if (rangeNode.Values.Count > 1)
            {
                var numbers = rangeNode.Values.Select(x => (NumberNode)x);
                quantifier.From = numbers.Min(x => x.Value);
                quantifier.To = numbers.Max(x => x.Value);
                quantifier.Kind = "Range";
            }

            return quantifier;
        }

        public IRegexNode ConvertString(StringNode stringNode)
        {
            return new RegexChar()
            {
                Symbol = Convert.ToChar(stringNode.Value.ToString()),
                Value = stringNode.Value
            };
        }

        public IRegexNode ConvertToChar(CharacterNode characterNode)
        {
            return new RegexChar()
            {
                Symbol = characterNode.Value,
                Value = characterNode.Value.ToString()
            };
        }

        public IRegexNode ConvertToCharacterClass(CharacterRangeNode rangeNode)
        {
            RegexCharacterClass characterClass = new RegexCharacterClass();

            foreach (CharacterNode node in rangeNode.Characters)
            {
                characterClass.Expressions.Add(node.Accept(this));
            }

            return characterClass;
        }

        public IRegexNode ConvertToGroup(GroupNode groupNode)
        {
            RegexGroup regexGroup = new();
            
            if (groupNode.Nodes.ToList().Any(n => n.GetType() == typeof(StringNode)))
            {
                groupNode = BreakUpStringNode(groupNode);
            }
            if (groupNode.Nodes.Count == 1)
            {
                INode node = groupNode.Nodes.First();
                regexGroup.Expressions.Add(node.Accept(this));
            }
            if (groupNode.Nodes.Count > 1)
            {
                RegexAlternative alternative = new RegexAlternative();
                alternative.Expressions = groupNode.Nodes.Select(node => node.Accept(this)).ToList();
                regexGroup.Expressions.Add(alternative);
            }
            return regexGroup;
        }

        private GroupNode BreakUpStringNode(GroupNode groupNode)
        {
            foreach (INode node in groupNode.Nodes.ToList())
            {
                if (node.GetType() == typeof(StringNode))
                {
                    StringNode stringNode = (StringNode)node;
                    foreach (char c in stringNode.Value.ToCharArray())
                    {
                        groupNode.Nodes.Add(new StringNode() { Value = c.ToString() });
                    }
                    groupNode.Nodes.Remove(stringNode);
                }
            }
            return groupNode;
        }
    }
}
