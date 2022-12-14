using RuddyRex.Lib.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace RuddyRex.Lib.Models
{
    public class AbstractTree<T> where T : class
    {
        public string Type { get; set; } = "Program";
        public List<T> Nodes { get; set; } = new List<T>();

        //public override bool Equals(object? obj)
        //{
        //    return obj is T tree &&
        //           Type == tree.Type &&
        //           EqualityComparer<List<INode>>.Default.Equals(Nodes, tree.Nodes);
        //}

        //public override int GetHashCode()
        //{
        //    return HashCode.Combine(Type, Nodes);
        //}
    }
}
