using RuddyRex.ParserLayer.Interfaces;

namespace RuddyRex.ParserLayer.Models;

public class CharacterRangeNode : INode
{
    public NodeType Type { get; } = NodeType.CharacterRange;
    public List<INode> Characters { get; set; } = new();

    public override bool Equals(object? obj)
    {
        return obj is CharacterRangeNode node &&
               Type == node.Type &&
               Characters.SequenceEqual(node.Characters); ;
    }

    public IRegexNode Accept(IConvorterVisitor visitor)
    {
        return visitor.ConvertToCharacterClass(this);
    }
}
