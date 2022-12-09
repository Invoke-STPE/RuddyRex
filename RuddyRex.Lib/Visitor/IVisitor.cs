using RuddyRex.Lib.Models.Interfaces;
using RuddyRex.Lib.Models.NodeModels;

namespace RuddyRex.Lib.Visitor
{
    public interface IVisitor
    {
        IRegexNode ConvertChar(CharacterNode characterNode);
        IRegexNode ConvertCharacterClass(CharacterRangeNode rangeNode);

        //IRegexNode ConvertGroup();
        //IRegexNode ConvertGroup(INode node);
        IRegexNode ConvertGroup(GroupNode groupNode);
        IRegexNode ConvertKeyword(KeywordNode keywordNode);
        IRegexNode ConvertRange(RangeNode rangeNode);
    }
}