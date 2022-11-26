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
    }
}
