using CFlow.Parser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace CFlow.Test
{
    [TestClass]
    public class CFlowParseTests
    {
        [TestMethod]
        [TestCategory("flow")]
        public void FlowGraphManyChildren()
        {
            var node = CFlowParser.Parse(new List<string>(TestConstants.FourLevelGraphFlow.Split(new string[] { Environment.NewLine }, StringSplitOptions.None)));

            // Function object is main
            AssertHelper.AssertFunctionNode(node.Function,
                @"main",
                @"int (void)",
                @"c:\Users\Morten\Documents\Atmel Studio\7.0\LEDflasher0\LEDflasher0\led_flasher_main.c",
                6);

            // This graph is a root node with 3 childrens
            Assert.AreEqual(3, node.Children.Count);

            // Children
            AssertHelper.AssertFunctionNode(node.Children[0].Function,
                @"system_init",
                @"void (void)",
                @"C:\Users\Morten\Documents\Atmel Studio\7.0\LEDflasher0\LEDflasher0\atmel_start.c",
                70);

            // system_init child nodes
            Assert.AreEqual(5, node.Children[0].Children.Count);
            AssertHelper.AssertFunctionNode(node.Children[0].Children[0].Function,
                @"init_mcu",
                string.Empty,
                string.Empty,
                0);
            AssertHelper.AssertFunctionNode(node.Children[0].Children[1].Function,
                @"gpio_set_pin_direction",
                string.Empty,
                string.Empty,
                0);
            AssertHelper.AssertFunctionNode(node.Children[0].Children[2].Function,
                @"gpio_set_pin_level",
                string.Empty,
                string.Empty,
                0);
            AssertHelper.AssertFunctionNode(node.Children[0].Children[3].Function,
                @"gpio_set_pin_mux",
                string.Empty,
                string.Empty,
                0);
            AssertHelper.AssertFunctionNode(node.Children[0].Children[4].Function,
                @"app_init",
                @"void (void)",
                @"C:\Users\Morten\Documents\Atmel Studio\7.0\LEDflasher0\LEDflasher0\atmel_start.c",
                23);

            // app_init calls 1 child
            Assert.AreEqual(1, node.Children[0].Children[4].Children.Count);
            AssertHelper.AssertFunctionNode(node.Children[0].Children[4].Children[0].Function,
                @"delay_init",
                string.Empty,
                string.Empty,
                0);

            AssertHelper.AssertFunctionNode(node.Children[1].Function,
                @"delay_ms",
                string.Empty,
                string.Empty,
                0);
            AssertHelper.AssertFunctionNode(node.Children[2].Function,
                @"gpio_toggle_pin_level",
                string.Empty,
                string.Empty,
                0);

        }
        [TestMethod]
        [TestCategory("flow")]
        public void FlowGraphWithChildren()
        {
            var node = CFlowParser.Parse(new List<string>(TestConstants.TwoLevelGraphFlow.Split(new string[] { Environment.NewLine }, StringSplitOptions.None)));

            // Function object is main
            AssertHelper.AssertFunctionNode(node.Function,
                @"main",
                @"int (void)",
                @"c:\Users\Morten\Documents\Atmel Studio\7.0\LEDflasher0\LEDflasher0\led_flasher_main.c",
                6);

            // This graph is a root node with 3 childrens
            Assert.AreEqual(3, node.Children.Count);

            // Children
            AssertHelper.AssertFunctionNode(node.Children[0].Function,
                @"system_init",
                @"void (void)",
                @"C:\Users\Morten\Documents\Atmel Studio\7.0\LEDflasher0\LEDflasher0\atmel_start.c",
                70);
            AssertHelper.AssertFunctionNode(node.Children[1].Function,
                @"delay_ms",
                string.Empty,
                string.Empty,
                0);
            AssertHelper.AssertFunctionNode(node.Children[2].Function,
                @"gpio_toggle_pin_level",
                string.Empty,
                string.Empty,
                0);
        }

        [TestMethod]
        [TestCategory("flow")]
        public void SimpleFlowGraph()
        {
            var node = CFlowParser.Parse(new List<string>(TestConstants.NoGraph.Split(new string[] { Environment.NewLine }, StringSplitOptions.None)));

            // Function object is main
            AssertHelper.AssertFunctionNode(node.Function, 
                @"main", 
                @"int (void)",
                @"c:\Users\Morten\Documents\Atmel Studio\7.0\GccApplication1\GccApplication1\main.cpp", 
                14);

            // This graph is 1 root node, no children
            Assert.AreEqual(0, node.Children.Count);

        }

        
    }
}
