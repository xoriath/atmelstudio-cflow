using Microsoft.VisualStudio.TestTools.UnitTesting;
using CFlow.Parser;

namespace CFlow.Tests
{
    [TestClass]
    public class FunctionTests
    {
        [TestMethod]
        [TestCategory("function")]
        public void FunctionEmpty()
        {
            var f = Function.Create(string.Empty, string.Empty, string.Empty, "0");

            Assert.AreEqual(string.Empty, f.File);
            Assert.AreEqual(string.Empty, f.Name);
            Assert.AreEqual(string.Empty, f.Signature);
            Assert.AreEqual((uint)0, f.Line);

        }

        [TestMethod]
        [TestCategory("function")]
        public void FunctionWithData()
        {
            var file = @"C:\Dev\test.c";
            var name = @"main";
            var signature = @"void (void)";
            var line = @"23";

            var f = Function.Create(name, signature, file, line);

            Assert.AreEqual(file, f.File);
            Assert.AreEqual(name, f.Name);
            Assert.AreEqual(signature, f.Signature);
            Assert.AreEqual((uint)23, f.Line);
        }

        [TestMethod]
        [TestCategory("function")]
        public void FunctionWithInvalidLine()
        {
            var file = @"C:\Dev\test.c";
            var name = @"main";
            var signature = @"void (void)";
            var line = @"-109";

            var f = Function.Create(name, signature, file, line);

            Assert.IsNull(f);
        }
    }
}