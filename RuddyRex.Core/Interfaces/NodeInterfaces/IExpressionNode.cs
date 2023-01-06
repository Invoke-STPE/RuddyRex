
using RuddyRex.Core.Interfaces.NodeInterfaces;

namespace RuddyRex.Core.Interfaces.NodeInterface
{
    public interface IExpressionNode : INode
    {
        string Keyword { get; set; }
        INode Parameter { get; set; }
        IStringValueNode ValueType { get; set; }

        bool Equals(object? obj);
        bool IsExactlyKeyword();
    }
}