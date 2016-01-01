using CFlow.Parser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CFlow.Test
{
    [TestClass]
    public class ParseReverseTests
    {
        [TestMethod]
        [TestCategory("flow-reverse")]
        public void SimpleReverse()
        {
            var references = CFlowReverseParser.Parse(new List<string>(TestConstants.SimpleReverse.Split(new string[] { Environment.NewLine }, StringSplitOptions.None)));

            Assert.AreEqual(1, references.Count);

            var app_init = references.First();
            AssertHelper.AssertFunctionNode(app_init.Function,
                @"app_init",
                @"void (void)",
                @"C:\Users\Morten\Documents\Atmel Studio\7.0\LEDflasher0\LEDflasher0\atmel_start.c",
                23);

            Assert.AreEqual(1, app_init.Parents.Count);

            var system_init = app_init.Parents.First();
            AssertHelper.AssertFunctionNode(system_init.Function,
                @"system_init",
                @"void (void)",
                @"C:\Users\Morten\Documents\Atmel Studio\7.0\LEDflasher0\LEDflasher0\atmel_start.c",
                70);

            Assert.AreEqual(1, system_init.Parents.Count);

            var main = system_init.Parents.First();
            AssertHelper.AssertFunctionNode(main.Function,
                @"main",
                @"int (void)",
                @"c:\Users\Morten\Documents\Atmel Studio\7.0\LEDflasher0\LEDflasher0\led_flasher_main.c",
                6);

        }
        [TestMethod]
        [TestCategory("flow-reverse")]
        public void ComplexReverse()
        {
            var references = CFlowReverseParser.Parse(new List<string>(TestConstants.ComplexReverse.Split(new string[] { Environment.NewLine }, StringSplitOptions.None)));

            
        }
    }
}
