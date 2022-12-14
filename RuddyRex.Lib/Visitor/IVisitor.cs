using RuddyRex.Lib.Models.Interfaces;
using RuddyRex.Lib.Models.NodeModels;

namespace RuddyRex.Lib.Visitor
{
    public interface IVisitor
    {
        IRegexNode ConvertToChar(CharacterNode characterNode);
        IRegexNode ConvertToCharacterClass(CharacterRangeNode rangeNode);
        IRegexNode ConvertToGroup(GroupNode groupNode);
        IRegexNode ConvertKeyword(KeywordNode keywordNode);
        IRegexNode ConvertRange(RangeNode rangeNode);
        IRegexNode ConvertString(StringNode stringNode);
    }
}