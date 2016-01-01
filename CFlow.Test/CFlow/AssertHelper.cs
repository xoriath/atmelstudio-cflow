
using CFlow.Parser;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CFlow.Test
{
    public class AssertHelper
    {
        public static void AssertFunctionNode(Function node, string name, string signature, string file, uint line)
        {
            Assert.AreEqual(name, node.Name);
            Assert.AreEqual(signature, node.Signature);
            Assert.AreEqual(file, node.File);
            Assert.AreEqual(line, node.Line);
        }
    }
}
