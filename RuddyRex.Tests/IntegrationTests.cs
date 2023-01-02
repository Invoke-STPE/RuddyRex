using Microsoft.VisualStudio.TestTools.UnitTesting;
using RuddyRex.CodeGeneration;
using RuddyRex.LexerLayer;
using RuddyRex.ParserLayer;
using RuddyRex.ParserLayer.Interfaces;
using RuddyRex.Transformation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuddyRex.Tests
{
    [TestClass]
    public class IntegrationTests
    {
        [TestMethod]
        public void IntegrationTest1()
        {
            string input = @"Match 
                            Between {2 till 4} digit ""-""{0 till 1}
                            Between {2 till 4} digit ""-""{0 till 1}
                            Between {2 till 4} digit ""-""{0 till 1}
                            Exactly {4} digit";
            string expected = "[1-9]{2,4}-?[1-9]{2,4}-?[1-9]{2,4}-?[1-9]{4}";
            var tokens = Lexer.Tokenize(input);
            var ast = Parser.ParseTree(tokens);
            IConvorterVisitor convorterVisitor = new RegexConvertorVisitor();
            var transformer = new Transformer(convorterVisitor);
            var regexTree = transformer.TransformTree(ast);
            var optimizedTree = CodeOptimizer.OptimizeTree(regexTree);
            var actual = CodeGenerator.GenerateCode(optimizedTree);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void IntegrationTest2()
        {
            string input = @"Match 
                            ""Tracking ID:"" space {0 till}
                            (Between {0 till} letter) space{0 till}";
            string expected = "Tracking ID: *([a-zA-Z]*) *";
            var tokens = Lexer.Tokenize(input);
            var ast = Parser.ParseTree(tokens);
            IConvorterVisitor convorterVisitor = new RegexConvertorVisitor();
            var transformer = new Transformer(convorterVisitor);
            var regexTree = transformer.TransformTree(ast);
            var optimizedTree = CodeOptimizer.OptimizeTree(regexTree);
            var actual = CodeGenerator.GenerateCode(optimizedTree);

            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void IntegrationTest3()
        {
            string input = @"Match 
                            (Exactly {3} digit) space
                            Exactly {3} digit ""-""{0 till 1}
                            Exactly {4} digit";
            string expected = "([1-9]{3}) [1-9]{3}-?[1-9]{4}";
            var tokens = Lexer.Tokenize(input);
            var ast = Parser.ParseTree(tokens);
            IConvorterVisitor convorterVisitor = new RegexConvertorVisitor();
            var transformer = new Transformer(convorterVisitor);
            var regexTree = transformer.TransformTree(ast);
            var optimizedTree = CodeOptimizer.OptimizeTree(regexTree);
            var actual = CodeGenerator.GenerateCode(optimizedTree);

            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void IntegrationTest4()
        {
            string input = @"Match 
                            (Between {3 till} letter) space
                            (Between {1 till 3} digit Exactly {1} letter "",""{0 till 1}) space
                            (Any{1 till }"",""{0 till 1}) space
                            (Exactly {4} digit)
                            Alternate
                            (Between {3 till} letter) space
                            (Between {1 till 3} digit Exactly {1} letter "",""{0 till 1}) space
                            (Exactly {4} digit)";
            string expected = "([a-zA-Z]{3,}) ([1-9]{1,3}[a-zA-Z]{1},?) (.+,?) ([1-9]{4})|([a-zA-Z]{3,}) ([1-9]{1,3}[a-zA-Z]{1},?) ([1-9]{4})";
            var tokens = Lexer.Tokenize(input);
            var ast = Parser.ParseTree(tokens);
            IConvorterVisitor convorterVisitor = new RegexConvertorVisitor();
            var transformer = new Transformer(convorterVisitor);
            var regexTree = transformer.TransformTree(ast);
            var optimizedTree = CodeOptimizer.OptimizeTree(regexTree);
            var actual = CodeGenerator.GenerateCode(optimizedTree);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void IntegrationTest5()
        {
            string input = @"Match 
                            ("","") (Between {1 till 3} letter)";
            string expected = ",([a-zA-Z]{1,3})";
            var tokens = Lexer.Tokenize(input);
            var ast = Parser.ParseTree(tokens);
            IConvorterVisitor convorterVisitor = new RegexConvertorVisitor();
            var transformer = new Transformer(convorterVisitor);
            var regexTree = transformer.TransformTree(ast);
            var optimizedTree = CodeOptimizer.OptimizeTree(regexTree);
            var actual = CodeGenerator.GenerateCode(optimizedTree);

            Assert.AreEqual(expected, actual);
        }

    }
}
