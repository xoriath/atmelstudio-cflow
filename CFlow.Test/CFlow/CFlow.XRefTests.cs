using CFlow.Parser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CFlow.Test
{
    [TestClass]
    public class CFlow
    {
        [TestMethod]
        [TestCategory("xref")]
        public void SimpleXref()
        {
            var references = CFlowXrefParser.Parse(new List<string>(TestConstants.SimpleXref.Split(new string[] { Environment.NewLine }, StringSplitOptions.None)));

            Assert.AreEqual(1, references.Count);

            var f = references.First();

            Assert.AreEqual("main", f.Name);
            Assert.AreEqual(true, f.IsPrimary);
            Assert.AreEqual(@"c:\Users\Morten\Documents\Atmel Studio\7.0\GccApplication1\GccApplication1\main.cpp", f.File);
            Assert.AreEqual(14, f.Line);
            Assert.AreEqual("int (void)", f.Signature);

            // No cross-references
            Assert.AreEqual(0, f.References.Count);

        }

        [TestMethod]
        [TestCategory("xref")]
        public void ComplexXref()
        {
            var references = CFlowXrefParser.Parse(new List<string>(TestConstants.ComplexXref.Split(new string[] { Environment.NewLine }, StringSplitOptions.None)));

            Assert.AreEqual(8, references.Count);

            // Only 1 on top
            Assert.AreEqual(1, references.Where(f => f.Name.Equals("app_init")).Count());

            // All top-level should be unique, check that Distinct is a No-Op
            Assert.AreEqual(references.Count, references.Distinct().Count());

            // Take out app_init (1 primary, 1 secondary)
            var app_init = references.Where(f => f.Name.Equals("app_init")).First();
            Assert.AreEqual(1, app_init.References.Count);
        }

        [TestMethod]
        [TestCategory("xref")]
        public void UnboundFunctionXref()
        {
            var references = CFlowXrefParser.ParseUnbound(new List<string>(TestConstants.ComplexXref.Split(new string[] { Environment.NewLine }, StringSplitOptions.None)));

            Assert.AreEqual(4, references["Default_Handler"].Count);
            Assert.AreEqual(4, references["handler"].Count);
            Assert.AreEqual(1, references["delay_init"].Count);
            Assert.AreEqual(1, references["init_mcu"].Count);
            Assert.AreEqual(2, references["delay_ms"].Count);
        }
    }
}
