using RuddyRex.Lib.Models;
using RuddyRex.Lib.Models.Interfaces;
using RuddyRex.Lib.Models.RegexModels;
using RuddyRex.Lib.Visitor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuddyRex.Lib
{
    public static class RuddyToRegexConvertor
    {
        private static RegexConvertorVisitor _regexConvertor = new RegexConvertorVisitor();
        public static List<IRegexNode> ConvertTree(AbstractTree tree)
        {
            List<IRegexNode> regexNodes = new();
            foreach (var node in tree.Nodes)
            {
                IRegexNode regexNode = TraverseNode(node);
                regexNodes.Add(regexNode);
            }

            return regexNodes;
        }

        public static List<IRegexNode> TraverseArray(List<INode> nodes)
        {
            List<IRegexNode> regexNodes = new();
            foreach (var node in nodes)
            {
                regexNodes.Add(node.OnEnter(_regexConvertor));
            }

            return regexNodes;
        }

        public static IRegexNode TraverseNode(INode node)
        {
            IRegexNode regexNode = node.OnEnter(_regexConvertor);
            //if (node.)
            return regexNode;
        }
    }
}
