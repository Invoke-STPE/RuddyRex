using RuddyRex.Lib.Enums;
using RuddyRex.Lib.Models.Interfaces;
using RuddyRex.Lib.Models.NodeModels;
using RuddyRex.Lib.Models.RegexModels;
using RuddyRex.Lib.Models.TokenModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace RuddyRex.Lib.Visitor
{
    public class RegexConvertorVisitor : IVisitor
    {
        public IRegexNode ConvertToGroup(GroupNode groupNode)
        {
            var regexGroup = new RegexGroup() { Type = RegexType.Group };
            if (groupNode.Nodes.Count > 0)
            {
                regexGroup.Expressions = Traverser.TraverseArray(groupNode.Nodes);
            }
            return regexGroup;
        }

        public IRegexNode ConvertToCharacterClass(CharacterRangeNode rangeNode)
        {
            var regexCharacterClass = new RegexCharacterClass() { Type = RegexType.CharacterClass };
            if (rangeNode.Characters.Count > 0)
            {
                regexCharacterClass.Expressions = Traverser.TraverseArray(rangeNode.Characters);
            }
            return regexCharacterClass;
        }

        public IRegexNode ConvertToChar(CharacterNode characterNode)
        {
            return new RegexChar() { Type = RegexType.Char, Kind = "simple", Symbol = characterNode.Value, Value = characterNode.Value.ToString() };
        }

        public IRegexNode ConvertKeyword(KeywordExpressionNode keywordNode)
        {
            RegexRepetition regexRepetition = new RegexRepetition() { Type = RegexType.Repetition };

            if (RuddyRexDictionary.IsValidKeyword(keywordNode.Keyword))
            {
                if (keywordNode.ValueType.Value == "letter")
                {
                    regexRepetition.Expression = new RegexChar() { Type = RegexType.Char, Kind = "meta", Value = "[a-zA-Z]" };
                }
                else // Must be "digit"
                {
                    regexRepetition.Expression = new RegexChar() { Type = RegexType.Char, Kind = "meta", Value = "[0-9]" };
                }
                if (keywordNode.Parameter is not null)
                {
                    regexRepetition.Quantifier = (RegexQuantifier)Traverser.TraverseNode(keywordNode.Parameter);
                    if (keywordNode.Keyword.ToLower() == "between")
                    {
                        RangeNode rangeNode = (RangeNode)keywordNode.Parameter;
                        if (rangeNode.Values.Count is 1) // If only one number in rangeNode from must be infinity
                        {
                            regexRepetition.Quantifier.To = 0;
                        }
                    } 
                }
            }
            return regexRepetition;
        }

        public IRegexNode ConvertRange(RangeNode rangeNode)
        {
            RegexQuantifier quantifier = new RegexQuantifier() { Type = RegexType.Quantifier, Kind = "range" };
            if (rangeNode.Values.Count > 0)
            {
                var numberTokens = rangeNode.Values.Select(x => (TokenNumber)x);
                quantifier.From = numberTokens.Min(x => x.Value);
                quantifier.To = numberTokens.Max(x => x.Value);
            }

            return quantifier;
        }

        public IRegexNode ConvertString(StringNode node)
        {
            RegexAlternative regexAlternative = new() { Type = RegexType.Alternative };
            char[] charArray = node.Value.ToCharArray();

            if (charArray.Length > 0)
            {
                CharacterRangeNode characterRangeNode = new CharacterRangeNode();
                foreach (var c in charArray)
                {
                    characterRangeNode.Characters.Add(new CharacterNode());
                }
                regexAlternative.Expressions = Traverser.TraverseArray(characterRangeNode.Characters);
            }

            return regexAlternative;
            
        }
    }
}
