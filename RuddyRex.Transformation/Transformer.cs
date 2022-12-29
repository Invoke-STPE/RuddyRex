using RuddyRex.ParserLayer.Interfaces;
using RuddyRex.ParserLayer.Models;
using RuddyRex.Transformation.Models;

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
		AbstractTree<IRegexNode> output = new AbstractTree<IRegexNode>() { Type = "RegExp" };
		if (tree.Nodes.Count > 1)
		{
			bool isRepetition = tree.Nodes.ToList().Any(n => n.GetType() == typeof(RangeNode));
			if (isRepetition)
			{
				RegexRepetition regexRepetition = new RegexRepetition();
				regexRepetition.Expression = tree.Nodes[0].Accept(_visitor);
				regexRepetition.Quantifier = (RegexQuantifier)tree.Nodes[1].Accept(_visitor);
                output.Nodes.Add(regexRepetition);
            }
			else
			{
                RegexAlternative regexNode = new RegexAlternative();
                foreach (var node in tree.Nodes)
                {
                    regexNode.Expressions.Add(node.Accept(_visitor));
                }
                output.Nodes.Add(regexNode);
            }
            
        }
        else
		{
            IRegexNode regexNode = tree.Nodes.First().Accept(_visitor);
            output.Nodes.Add(regexNode);
        }
		return output;
	}
}
