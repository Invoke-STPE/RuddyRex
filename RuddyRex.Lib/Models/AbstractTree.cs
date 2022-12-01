using RuddyRex.Lib.Models.NodeModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace RuddyRex.Lib.Models
{
    public class AbstractTree
    {
        public string Type { get; set; } = "Program";
        public List<INode> Nodes { get; set; } = new List<INode>();

        public override bool Equals(object? obj)
        {
            return obj is AbstractTree tree &&
                   Type == tree.Type &&
                   EqualityComparer<List<INode>>.Default.Equals(Nodes, tree.Nodes);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Type, Nodes);
        }
    }
}
