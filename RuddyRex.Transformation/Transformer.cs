using RuddyRex.ParserLayer;
using RuddyRex.ParserLayer.Interfaces;
using RuddyRex.ParserLayer.Models;
using RuddyRex.Transformation.Models;
using System.Xml.Linq;

namespace RuddyRex.Transformation;
public class Transformer
{
	private readonly IConvorterVisitor _visitor;

	public Transformer(IConvorterVisitor visitor)
	{
		_visitor = visitor;
	}

	public AbstractTree<IRegexNode> TransformTree(AbstractTree<INode> tree)
	{
		AbstractTree<INode> brokenStringNodes = new();

		brokenStringNodes.Nodes.AddRange(BreakUpStringNodes(tree));

        AbstractTree<IRegexNode> output = new() { Type = "RegExp" };
        if (brokenStringNodes.Nodes.Count == 1)
        {
            var firstNode = brokenStringNodes.Nodes.First();
            output.Nodes.Add(
                firstNode
                .Accept(_visitor)
                );

            return output;
        }
        if (brokenStringNodes.Nodes.Count == 2)
        {
            int rangeCount = brokenStringNodes.Nodes.Count(n => n.Type == NodeType.RangeExpression);
            if (rangeCount == 1)
            {
                RegexRepetition repetition = new()
                {
                    Expression = brokenStringNodes.Nodes[0].Accept(_visitor),
                    Quantifier = (RegexQuantifier)brokenStringNodes.Nodes[1].Accept(_visitor)
                };

                output.Nodes.Add(repetition);
                return output;
            }
        }

        RegexAlternative alternative = new();

        foreach (var node in brokenStringNodes.Nodes)
        {
            alternative.Expressions.Add(node.Accept(_visitor));
        }
        output.Nodes.Add(alternative);
		return output;
	}

	private List<INode> BreakUpStringNodes(AbstractTree<INode> tree)
	{
        List<INode> output = new();
		foreach (var node in tree.Nodes)
		{
			output.AddRange(BreakNode(node));
		}
        return output;
	}

	private List<INode> BreakNode(INode node)
	{
        List<INode> output = new();
        switch (node.Type)
        {
            case NodeType.StringLiteral:
                StringNode stringNode = (StringNode)node;
                foreach (var s in stringNode.Value)
                {
                    output.Add(new StringNode() { Value = s.ToString() });
                }
                break;
            case NodeType.GroupExpression:
                GroupNode groupNode = (GroupNode)node;
                GroupNode newGroup = new GroupNode();
                foreach (var group in groupNode.Nodes)
                {
                    newGroup.Nodes.AddRange(BreakNode(group));
                }
                output.Add(newGroup);
                break;
            default:
                output.Add(node);
                break;
        }

        return output;
    }
}
