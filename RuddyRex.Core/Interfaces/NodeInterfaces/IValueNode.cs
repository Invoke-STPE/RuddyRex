using RuddyRex.Core.Interfaces.NodeInterface;
using RuddyRex.Core.Interfaces.RegexInterface;
using RuddyRex.Core.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuddyRex.Core.Interfaces.NodeInterfaces
{
    public interface IStringValueNode : INode
    {
        public string Value { get; set; }
    }
}
