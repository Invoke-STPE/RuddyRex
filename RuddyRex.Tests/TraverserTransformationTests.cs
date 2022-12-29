using RuddyRex.Lib;
using System;
using Newtonsoft.Json.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RuddyRex.Transformation;
using RuddyRex.ParserLayer.Models;
using RuddyRex.ParserLayer.Interfaces;
using RuddyRex.Transformation.Models;

namespace RuddyRex.Tests
{
    [TestClass]
    public class TraverserShouldReturnChainedExpression
    {
        private RegexConvertorVisitor _convertor;
        private Transformer _traverser;

        [TestInitialize]
        public void InitializeTest()
        {
            _convertor = new RegexConvertorVisitor();
            _traverser = new Transformer(_convertor);
        }

        [TestMethod]
        public void WhenPassedChainGroupAndBracket_ReturnsGroupAndCharacterClass()
        {
            AbstractTree<INode> input = new AbstractTree<INode>()
            {
                Nodes = new List<INode>()
                {
                    new GroupNode(),
                    new CharacterRangeNode(),
                }
            };

            AbstractTree<IRegexNode> expected = new AbstractTree<IRegexNode>()
            {
                Type = "RegExp",
                Nodes = new List<IRegexNode>()
                {
                    new RegexAlternative()
                    {
                        Expressions = new List<IRegexNode>()
                        {
                            new RegexGroup(),
                            new RegexCharacterClass()
                        }
                    }
                }
            };

            AbstractTree<IRegexNode> actual = _traverser.TransformTree(input);

            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void WhenPassedSingleNode_DoNotReturnAlternative()
        {
            AbstractTree<INode> input = new AbstractTree<INode>()
            {
                Nodes = new List<INode>()
                {
                    new GroupNode(),
                }
            };

            AbstractTree<IRegexNode> expected = new AbstractTree<IRegexNode>()
            {
                Type = "RegExp",
                Nodes = new List<IRegexNode>()
                {
                   new RegexGroup()
                }
            };

            AbstractTree<IRegexNode> actual = _traverser.TransformTree(input);

            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void WhenPassedNodeAndRange_ReturnsRepetition()
        {
            AbstractTree<INode> input = new AbstractTree<INode>()
            {
                Nodes = new List<INode>()
                {
                    new GroupNode(),
                    new RangeNode()
                    {
                        Values = new List<INode>()
                        {
                            new NumberNode() { Value = 1 },
                            new NumberNode() { Value = 2 }
                        }
                    },
                }
            };

            AbstractTree<IRegexNode> expected = new AbstractTree<IRegexNode>()
            {
                Type = "RegExp",
                Nodes = new List<IRegexNode>()
                {
                    new RegexRepetition()
                    {
                        Expression = new RegexGroup(),
                        Quantifier = new RegexQuantifier() {Kind = "Range", To = 2, From = 1}
                    }
                }
            };

            AbstractTree<IRegexNode> actual = _traverser.TransformTree(input);

            Assert.AreEqual(expected, actual);
        }
    }

}
