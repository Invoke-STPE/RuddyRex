using RuddyRex.ParserLayer.Models;

namespace RuddyRex.ParserLayer.Interfaces;

public interface IConvorterVisitor
{
    IRegexNode ConvertToChar(CharacterNode characterNode);
    IRegexNode ConvertToCharacterClass(CharacterRangeNode rangeNode);
    IRegexNode ConvertToGroup(GroupNode groupNode);
    IRegexNode ConvertKeyword(KeywordExpressionNode keywordNode);
    IRegexNode ConvertRange(RangeNode rangeNode);
    IRegexNode ConvertString(StringNode stringNode);
}