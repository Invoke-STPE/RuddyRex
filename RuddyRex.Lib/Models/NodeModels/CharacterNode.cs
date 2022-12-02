﻿using RuddyRex.Lib.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuddyRex.Lib.Models.NodeModels
{
    public class CharacterNode : INode
    {
        public NodeType Type { get; set; }
        public char Value { get; set; }
    }
}
