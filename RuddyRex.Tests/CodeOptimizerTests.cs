using Microsoft.VisualStudio.TestTools.UnitTesting;
using RuddyRex.Lib.Enums;
using RuddyRex.Lib.Models;
using RuddyRex.Lib.Models.Interfaces;
using RuddyRex.Lib.Models.RegexModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuddyRex.Tests
{
    [TestClass]
    public class CodeOptimizerShouldOptimizeGroup
    {
        [TestMethod]
        public void WhenPassedSuperficialGroups_ReturnsRemovedGroup()
        {
            AbstractTree<IRegexNode> abstractTree = new() 
            { 
                Type = "RegExp",
                Nodes = new List<IRegexNode>() 
                {
                    new RegexGroup() 
                    { 
                        Type = RegexType.Group,
                        Expressions = new List<IRegexNode>()
                        {
                            new RegexChar() { }
                        }
                    },
                }
            };
        }
    }
}
