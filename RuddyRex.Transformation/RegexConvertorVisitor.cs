using RuddyRex.Core.Exceptions;
using RuddyRex.Core.Interfaces.NodeInterface;
using RuddyRex.Core.Interfaces.NodeInterfaces;
using RuddyRex.Core.Interfaces.RegexInterface;
using RuddyRex.Core.Interfaces.VisitorInterfaces;
using RuddyRex.Core.Types;
using RuddyRex.Transformation.Models;
using RuddyRex.Transformation.Models.DTO;

namespace RuddyRex.Transformation
{
    public class RegexConvertorVisitor : IConvorterVisitor
    {
        public static Stack<INode> Stack { get; set; } = new Stack<INode>();
        public RegexConvertorVisitor()
        {
            Stack = new Stack<INode>();
        }
        public RegexConvertorVisitor(IExpressionNode keyword)
        {
            Stack.Push(keyword);
        }
        public IRegexNode ConvertKeywordExpression(IExpressionNode keywordNode)
        {
            RegexRepetition repetition = new();
            Stack.Push(keywordNode);

            repetition.Quantifier = (RegexQuantifier)keywordNode.Parameter.Accept(this);
            repetition.Expression = new RegexCharacterClass(keywordNode);

            return repetition;
        }

        public IRegexNode ConvertRange(IParentNode rangeNode)
        {
            RegexQuantifier quantifier = new RegexQuantifier();
            Stack.TryPeek(out INode result);
            IExpressionNode? keyword = result?.Type == NodeType.KeywordExpression ? (IExpressionNode)Stack.Pop() : new KeywordExpressionNodeDTO();
            if (rangeNode.Nodes.Count == 1)
            {
                var numbers = rangeNode.Nodes.Select(x => (INumberNode)x);
                quantifier.From = numbers.Min(x => x.Value);
                quantifier.To = numbers.Min(x => x.Value);
                quantifier.Kind = "Range";

                if (keyword.IsExactlyKeyword())
                {
                    return quantifier;
                }
                

                if (quantifier.IsAsteriskRange())
                {
                    quantifier.Kind = "*";
                    return quantifier;
                }
                if (quantifier.IsPlusRange())
                {
                    quantifier.Kind = "+";
                    quantifier.To = 0;
                    quantifier.From = 0;
                    return quantifier;
                }

            }
            if (rangeNode.Nodes.Count > 1)
            {
                var numbers = rangeNode.Nodes.Select(x => (INumberNode)x);
                quantifier.From = numbers.Min(x => x.Value);
                quantifier.To = numbers.Max(x => x.Value);
                quantifier.Kind = "Range";
                if (quantifier.IsQuestionMarkRange())
                {
                    quantifier.Kind = "?";
                    quantifier.To = 0;
                    quantifier.From = 0;
                    return quantifier;
                }
                return quantifier;
            }


            quantifier.To = 0;
            return quantifier;
        }

        public IRegexNode ConvertString(IStringValueNode stringNode)
        {
            Stack.Push(stringNode);
            return new RegexChar()
            {
                Symbol = Convert.ToChar(stringNode.Value.ToString()),
                Value = stringNode.Value
            };
        }

        public IRegexNode ConvertToChar(ICharValueNode characterNode)
        {
            Stack.Push(characterNode);
            return new RegexChar()
            {
                Symbol = characterNode.Value,
                Value = characterNode.Value.ToString()
            };
        }

        public IRegexNode ConvertToCharacterClass(IParentNode rangeNode)
        {
            RegexCharacterClass characterClass = new RegexCharacterClass();
            Stack.Push(rangeNode);
            foreach (ICharValueNode node in rangeNode.Nodes)
            {
                characterClass.Expressions.Add(node.Accept(this));
            }

            return characterClass;
        }

        public IRegexNode ConvertToGroup(IParentNode groupNode)
        {
            RegexGroup regexGroup = new();
            Stack.Push(groupNode);
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

        private IParentNode BreakUpStringNode(IParentNode groupNode)
        {
            foreach (INode node in groupNode.Nodes.ToList())
            {
                if (node.GetType() == typeof(StringNodeDTO))
                {
                    StringNodeDTO stringNode = (StringNodeDTO)node;
                    foreach (char c in stringNode.Value.ToCharArray())
                    {
                        groupNode.Nodes.Add(new StringNodeDTO() { Value = c.ToString() });
                    }
                    groupNode.Nodes.Remove(stringNode);
                }
            }
            return groupNode;
        }

        public IRegexNode ConvertKeyword(IStringValueNode keywordNode)
        {
            switch (keywordNode.Value.ToLower())
            {
                case "any":
                    return new RegexChar() { Kind = "simple", Value = ".", Symbol = '.' };
                case "alternate":
                    return new RegexChar() { Kind = "simple", Value = "|", Symbol = '|' };
                case "space":
                    return new RegexChar() { Kind = "simple", Value = " ", Symbol = ' ' };
                default:
                    throw new InvalidKeywordException("Invalid keyword");
            }
        }
    }
}
