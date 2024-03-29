﻿using RuddyRex.Core.Interfaces.NodeInterface;
using RuddyRex.Core.Interfaces.RegexInterface;
using RuddyRex.Core;
using RuddyRex.ParserLayer.Models;
using RuddyRex.Transformation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuddyRex.Transformation.Tests
{
    [TestClass]
    public class TransformationTests
    {


        [TestClass]
        public class TransformerShouldAppend : TransformationTests
        {
            [TestMethod]
            public void SingleCharOntoTree()
            {
                AbstractTree<INode> input = new AbstractTree<INode>()
                {
                    Nodes = new List<INode>()
                    {
                        new CharacterNode() { Value = 'a' },
                    }
                };

                AbstractTree<IRegexNode> expected = new()
                {
                    Type = "RegExp",
                    Nodes = new List<IRegexNode>()
                    {
                        new RegexChar() { Kind = "simple", Symbol = 'a', Value = "a" },
                    }
                };

                var actual = Transformer.TransformTree(input);

                Assert.AreEqual(expected, actual);
            }
            [TestMethod]
            public void SingleGroupOntoTree()
            {
                AbstractTree<INode> input = new AbstractTree<INode>()
                {
                    Nodes = new List<INode>()
                    {
                        new GroupNode(),
                    }
                };

                AbstractTree<IRegexNode> expected = new()
                {
                    Type = "RegExp",
                    Nodes = new List<IRegexNode>()
                    {
                        new RegexGroup(),
                    }
                };

                var actual = Transformer.TransformTree(input);

                Assert.AreEqual(expected, actual);
            }
            [TestMethod]
            public void SingleCharacterOntoTree()
            {
                AbstractTree<INode> input = new AbstractTree<INode>()
                {
                    Nodes = new List<INode>()
                    {
                        new CharacterRangeNode()
                        {
                            Nodes = new List<INode>()
                            {
                                new CharacterNode() {Value = 'a'}
                            }
                        },
                    }
                };

                AbstractTree<IRegexNode> expected = new()
                {
                    Type = "RegExp",
                    Nodes = new List<IRegexNode>()
                    {
                        new RegexCharacterClass()
                        {
                            Expressions = new List<IRegexNode>()
                            {
                                new RegexChar() { Kind = "simple", Symbol = 'a', Value = "a" },
                            }
                        }
                    }
                };

                var actual = Transformer.TransformTree(input);

                Assert.AreEqual(expected, actual);
            }
        }

        [TestClass]
        public class TransformerShouldAppendRepetition : TransformationTests
        {
            [TestMethod]
            public void WhenPassedCharWithRange_ReturnsRepetition()
            {
                AbstractTree<INode> input = new()
                {
                    Nodes = new List<INode>()
                    {
                        new StringNode() { Value = "a"},
                        new RangeNode()
                        {
                            Nodes = new List<INode>()
                            {
                                new NumberNode(){ Value = 2},
                                new NumberNode(){ Value = 4},
                            }
                        }
                    }
                };

                AbstractTree<IRegexNode> expected = new()
                {
                    Type = "RegExp",
                    Nodes = new List<IRegexNode>()
                    {
                        new RegexRepetition()
                        {
                            Expression = new RegexChar() { Kind = "simple", Symbol = 'a', Value = "a" },
                            Quantifier = new RegexQuantifier()
                            {
                                From = 2,
                                To = 4,
                                Kind = "Range"
                            }
                        }
                    }
                };

                var actual = Transformer.TransformTree(input);

                Assert.AreEqual(expected, actual);
            }
            [TestMethod]
            public void WhenPassedCharacterClassWithRange_ReturnsRepetition()
            {
                AbstractTree<INode> input = new()
                {
                    Nodes = new List<INode>()
                    {
                       new CharacterRangeNode()
                       {
                           Nodes = new List<INode>()
                           {
                               new CharacterNode() { Value = 'a'},
                               new CharacterNode() { Value = 'b'},
                           }

                       },
                        new RangeNode()
                        {
                            Nodes = new List<INode>()
                            {
                                new NumberNode(){ Value = 2},
                                new NumberNode(){ Value = 4},
                            }
                        }
                    }
                };

                AbstractTree<IRegexNode> expected = new()
                {
                    Type = "RegExp",
                    Nodes = new List<IRegexNode>()
                    {
                        new RegexRepetition()
                        {
                            Expression = new RegexCharacterClass()
                            {
                                Expressions = new List<IRegexNode>()
                                {
                                    new RegexChar() { Kind = "simple", Symbol = 'a', Value = "a" },
                                    new RegexChar() { Kind = "simple", Symbol = 'b', Value = "b" },
                                }
                            },
                            Quantifier = new RegexQuantifier()
                            {
                                From = 2,
                                To = 4,
                                Kind = "Range"
                            }
                        }
                    }
                };

                var actual = Transformer.TransformTree(input);

                Assert.AreEqual(expected, actual);
            }
            [TestMethod]
            public void WhenPassedExpression_ReturnsRepetition()
            {
                AbstractTree<INode> input = new()
                {
                    Nodes = new List<INode>()
                    {
                       new KeywordExpressionNode()
                       {
                           Keyword = "Between",
                           Parameter = new RangeNode()
                           {
                               Nodes = new List<INode>()
                               {
                                   new NumberNode(){ Value = 2},
                                   new NumberNode(){ Value = 4},
                               }
                           },
                           ValueType = new KeywordNode() { Value = "letter"}
                       }
                    }
                };

                AbstractTree<IRegexNode> expected = new()
                {
                    Type = "RegExp",
                    Nodes = new List<IRegexNode>()
                    {
                        new RegexRepetition()
                        {
                            Expression = new RegexCharacterClass()
                            {
                                Expressions = new List<IRegexNode>()
                                {
                                    new RegexClassRange()
                                    {
                                        From = new RegexChar( ){ Kind = "simple", Value = "a-z", Symbol = 'a'},
                                        To = new RegexChar( ){ Kind = "simple", Value = "A-Z", Symbol = 'z'}
                                    }
                                }
                            },
                            Quantifier = new RegexQuantifier()
                            {
                                To = 4,
                                From = 2,
                                Kind = "Range"
                            }
                        }
                    }
                };

                var actual = Transformer.TransformTree(input);

                Assert.AreEqual(expected, actual);
            }

        }
        [TestClass]
        public class TransformerShouldAppendAlternative : TransformationTests
        {
            [TestMethod]
            public void WhenPassedTwoNodes_ReturnsAlternate()
            {
                AbstractTree<INode> input = new()
                {
                    Nodes = new List<INode>()
                    {
                       new CharacterRangeNode()
                       {
                           Nodes = new List<INode>()
                           {
                               new CharacterNode() { Value = 'a'},
                               new CharacterNode() { Value = 'b'},
                           }
                       },
                       new StringNode() { Value = "cb"}
                    }
                };

                AbstractTree<IRegexNode> expected = new()
                {
                    Type = "RegExp",
                    Nodes = new List<IRegexNode>()
                    {
                       new RegexAlternative()
                       {
                           Expressions = new List<IRegexNode>()
                           {
                               new RegexCharacterClass()
                               {
                                   Expressions = new List<IRegexNode>()
                                   {
                                       new RegexChar() { Kind = "simple", Symbol = 'a', Value = "a" },
                                       new RegexChar() { Kind = "simple", Symbol = 'b', Value = "b" },
                                   }
                               },
                               new RegexChar() { Kind = "simple", Symbol = 'c', Value = "c"},
                               new RegexChar() { Kind = "simple", Symbol = 'b', Value = "b"}
                           }
                       },
                    }
                };

                var actual = Transformer.TransformTree(input);

                Assert.AreEqual(expected, actual);
            }

            [TestMethod]
            public void WhenPassedNestedStringNodes_ReturnsAlternate()
            {
                AbstractTree<INode> input = new()
                {
                    Nodes = new List<INode>()
                    {
                       new GroupNode()
                       {
                           Nodes = new List<INode>()
                           {
                               new StringNode(){ Value = "ab"}
                           }
                       },
                       new StringNode() { Value = "cb"}
                    }
                };

                AbstractTree<IRegexNode> expected = new()
                {
                    Type = "RegExp",
                    Nodes = new List<IRegexNode>()
                    {
                       new RegexAlternative()
                       {
                           Expressions = new List<IRegexNode>()
                           {
                               new RegexGroup()
                               {
                                   Expressions = new List<IRegexNode>()
                                   {
                                       new RegexAlternative()
                                       {
                                           Expressions = new List<IRegexNode>()
                                           {
                                                 new RegexChar() { Kind = "simple", Symbol = 'a', Value = "a"},
                                                 new RegexChar() { Kind = "simple", Symbol = 'b', Value = "b"}
                                           }
                                       }

                                   }
                               },
                               new RegexChar() { Kind = "simple", Symbol = 'c', Value = "c"},
                               new RegexChar() { Kind = "simple", Symbol = 'b', Value = "b"}
                           }
                       },
                    }
                };

                var actual = Transformer.TransformTree(input);

                Assert.AreEqual(expected, actual);
            }

            [TestMethod]
            public void WhenPassedStringNode_ReturnsAlternate()
            {
                AbstractTree<INode> input = new()
                {
                    Nodes = new List<INode>()
                    {
                       new StringNode()
                       {
                           Value = "ab"
                       }
                    }
                };

                AbstractTree<IRegexNode> expected = new()
                {
                    Type = "RegExp",
                    Nodes = new List<IRegexNode>()
                    {
                       new RegexAlternative()
                       {
                           Expressions = new List<IRegexNode>()
                           {
                               new RegexChar( ){ Kind = "simple", Value = "a", Symbol = 'a'},
                               new RegexChar( ){ Kind = "simple", Value = "b", Symbol = 'b'}
                           }
                       },
                    }
                };

                var actual = Transformer.TransformTree(input);

                Assert.AreEqual(expected, actual);
            }
        }
        [TestClass]
        public class TransformerShouldTransformGroup : TransformationTests
        {
            [TestMethod]
            public void WhenPassedGroupWithTwoExpressions()
            {
                var input = new AbstractTree<INode>()
                {
                    Type = "Match",
                    Nodes = new List<INode>()
                    {
                        new GroupNode()
                        {
                            Nodes = new List<INode>()
                            {
                                new KeywordExpressionNode()
                                {
                                    Keyword = "Between",
                                    Parameter = new RangeNode()
                                    {
                                        Nodes = new List<INode>()
                                        {
                                            new NumberNode() { Value = 1},
                                            new NumberNode() { Value = 3}
                                        }
                                    },
                                    ValueType = new KeywordNode(){ Value = "digit"}
                                },
                                new KeywordExpressionNode()
                                {
                                    Keyword = "Exactly",
                                    Parameter = new RangeNode()
                                    {
                                        Nodes = new List<INode>()
                                        {
                                            new NumberNode() { Value = 1},
                                        }
                                    },
                                    ValueType = new KeywordNode(){ Value = "letter"}
                                }
                            }
                        }
                    }
                };

                AbstractTree<IRegexNode> expected = new AbstractTree<IRegexNode>()
                {
                    Type = "RegExp",
                    Nodes = new List<IRegexNode>()
                    {
                        new RegexGroup()
                        {
                            Expressions = new List<IRegexNode>()
                            {
                                new RegexAlternative()
                                {
                                    Expressions = new List<IRegexNode>()
                                    {
                                        new RegexRepetition()
                                        {
                                           Expression = new RegexCharacterClass()
                                            {
                                                Expressions = new List<IRegexNode>()
                                                {
                                                    new RegexClassRange()
                                                    {
                                                        From = new RegexChar( ){ Kind = "simple", Value = "0", Symbol = '0'},
                                                        To = new RegexChar( ){ Kind = "simple", Value = "9", Symbol = '9'}
                                                    }
                                                }
                                            },
                                            Quantifier = new RegexQuantifier()
                                            {
                                                Kind = "Range",
                                                From = 1,
                                                To = 3

                                            }
                                        },
                                        new RegexRepetition()
                                        {
                                           Expression = new RegexCharacterClass()
                                            {
                                                Expressions = new List<IRegexNode>()
                                                {
                                                    new RegexClassRange()
                                                    {
                                                        From = new RegexChar( ){ Kind = "simple", Value = "a-z", Symbol = 'a'},
                                                        To = new RegexChar( ){ Kind = "simple", Value = "A-Z", Symbol = 'z'}
                                                    }
                                                }
                                            },
                                            Quantifier = new RegexQuantifier()
                                            {
                                                Kind = "Range",
                                                To = 1,
                                                From = 1
                                            }
                                        }
                                    }

                                }
                            }
                        }
                    }
                };

                var actual = Transformer.TransformTree(input);

                Assert.AreEqual(expected, actual);
            }
            [TestMethod]
            public void WhenPassedTwoExpressionAndCharacterRange()
            {
                AbstractTree<INode> input = new()
                {
                    Type = "Match",
                    Nodes = new List<INode>()
                    {
                        new GroupNode()
                        {
                            Nodes = new List<INode>()
                            {
                                new KeywordExpressionNode()
                                {
                                    Keyword = "Between",
                                    Parameter = new RangeNode()
                                    {
                                        Nodes = new List<INode>()
                                        {
                                            new NumberNode() { Value = 1},
                                            new NumberNode() { Value = 3}
                                        }
                                    },
                                    ValueType = new KeywordNode(){ Value = "digit"}
                                },
                                new KeywordExpressionNode()
                                {
                                    Keyword = "Exactly",
                                    Parameter = new RangeNode()
                                    {
                                        Nodes = new List<INode>()
                                        {
                                            new NumberNode() { Value = 1},
                                        }
                                    },
                                    ValueType = new KeywordNode(){ Value = "letter"}
                                },
                                new StringNode(){ Value = ","},
                                new RangeNode()
                                {
                                    Nodes = new List<INode>()
                                    {
                                        new NumberNode() { Value = 0},
                                        new NumberNode() { Value = 1},
                                    }
                                }
                            }
                        }
                    }
                };

                AbstractTree<IRegexNode> expected = new AbstractTree<IRegexNode>()
                {
                    Type = "RegExp",
                    Nodes = new List<IRegexNode>()
                    {
                        new RegexGroup()
                        {
                            Expressions = new List<IRegexNode>()
                            {
                                new RegexAlternative()
                                {
                                    Expressions = new List<IRegexNode>()
                                    {
                                        new RegexRepetition()
                                        {
                                           Expression = new RegexCharacterClass()
                                            {
                                                Expressions = new List<IRegexNode>()
                                                {
                                                    new RegexClassRange()
                                                    {
                                                        From = new RegexChar( ){ Kind = "simple", Value = "0", Symbol = '0'},
                                                        To = new RegexChar( ){ Kind = "simple", Value = "9", Symbol = '9'}
                                                    }
                                                }
                                            },
                                            Quantifier = new RegexQuantifier()
                                            {
                                                Kind = "Range",
                                                From = 1,
                                                To = 3

                                            }
                                        },
                                        new RegexRepetition()
                                        {
                                           Expression = new RegexCharacterClass()
                                            {
                                                Expressions = new List<IRegexNode>()
                                                {
                                                    new RegexClassRange()
                                                    {
                                                        From = new RegexChar( ){ Kind = "simple", Value = "a-z", Symbol = 'a'},
                                                        To = new RegexChar( ){ Kind = "simple", Value = "A-Z", Symbol = 'z'}
                                                    }
                                                }
                                            },
                                            Quantifier = new RegexQuantifier()
                                            {
                                                Kind = "Range",
                                                To = 1,
                                                From = 1
                                            }
                                        },
                                        new RegexChar(){ Kind = "simple", Value = ",", Symbol = ','},
                                        new RegexQuantifier() { Kind = "?", From = 0, To = 0}
                                    }

                                }
                            }
                        }
                    }
                };

                var actual = Transformer.TransformTree(input);

                Assert.AreEqual(expected, actual);
            }
        }
    }
}
