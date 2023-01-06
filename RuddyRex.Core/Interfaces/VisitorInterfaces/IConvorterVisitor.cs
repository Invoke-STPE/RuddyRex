using RuddyRex.Core.Interfaces.NodeInterface;
using RuddyRex.Core.Interfaces.NodeInterfaces;
using RuddyRex.Core.Interfaces.RegexInterface;

namespace RuddyRex.Core.Interfaces.VisitorInterfaces;

public interface IConvorterVisitor
{
    IRegexNode ConvertToChar(ICharValueNode characterNode);
    IRegexNode ConvertToCharacterClass(IParentNode rangeNode);
    IRegexNode ConvertToGroup(IParentNode groupNode);
    IRegexNode ConvertKeywordExpression(IExpressionNode keywordNode);
    IRegexNode ConvertKeyword(IStringValueNode keywordNode);
    IRegexNode ConvertRange(IParentNode rangeNode);
    IRegexNode ConvertString(IStringValueNode stringNode);
}