﻿using RuddyRex.Core.Interfaces.NodeInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuddyRex.Core.Interfaces.NodeInterfaces
{
    public interface ICharValueNode : INode
    {
        public char Value { get; set; }
    }
}
