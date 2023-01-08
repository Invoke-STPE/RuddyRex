using RuddyRex.Core.Interfaces.NodeInterface;

namespace RuddyRex.Core.Interfaces.NodeInterfaces
{
    public interface IParentNode : INode
    {
        public List<INode> Nodes { get; set; }


    }
}
