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
    public static class Traverser
    {
        private static RegexConvertorVisitor _regexConvertor = new();
        public static AbstractTree<IRegexNode> ConvertTree(AbstractTree<INode> tree)
        {
            AbstractTree<IRegexNode> abstractTree = new() { Type = "RegExp" };
            List<IRegexNode> regexNodes = new();
            foreach (var node in tree.Nodes)
            {
                IRegexNode regexNode = TraverseNode(node);
                regexNodes.Add(regexNode);
            }
            abstractTree.Nodes = regexNodes;
            return abstractTree;
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
            return regexNode;
        }
    }
}
