//------------------------------------------------------------------------------
// <copyright file="ReverseFunctionWindowControl.xaml.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace CFlow
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for ReverseFunctionWindowControl.
    /// </summary>
    public partial class ReverseFunctionWindowControl : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReverseFunctionWindowControl"/> class.
        /// </summary>
        public ReverseFunctionWindowControl()
        {
            Load();

            try
            {
                InitializeComponent();
                graphLayout.Graph = ((FunctionViewModel)DataContext).Graph;
            }

            catch (Exception e)
            {
                VSHelper.Log(e);
            }
        }

        /// <summary>
        /// Handles click on the button by displaying a message box.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event args.</param>
        [SuppressMessage("Microsoft.Globalization", "CA1300:SpecifyMessageBoxOptions", Justification = "Sample code")]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Default event handler naming pattern")]
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Load();
        }

        private void Load()
        {
            var package = (CFlowPackage)ReverseFunctionWindowCommand.Instance.ServiceProvider;
            var projects = VSHelper.GetProjects(package.Dte);

            var project = projects.First();
            var files = VSHelper.GetFiles(project);
            files = files.Where(f => Path.GetExtension(f) != ".ld").ToList();


            var treeRunner = new Runner.TreeRunner(package.CFlow, files);
            var treeOutput = treeRunner.Run();
            var treeResults = Parser.CFlowParser.Parse(new List<string>(treeOutput.Split(new string[] { Environment.NewLine }, StringSplitOptions.None)));

            DataContext = new FunctionViewModel(treeResults);
            
        }
    }
}