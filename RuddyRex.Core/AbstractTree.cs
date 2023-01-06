namespace RuddyRex.Core;
public class AbstractTree<T> where T : class
{
    public string Type { get; set; } = "Program";
    public List<T> Nodes { get; set; } = new List<T>();

    public override bool Equals(object? obj)
    {
        return obj is AbstractTree<T> tree &&
               Type == tree.Type &&
               Nodes.SequenceEqual(tree.Nodes);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Type, Nodes);
    }
}
