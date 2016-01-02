//------------------------------------------------------------------------------
// <copyright file="ReverseFunctionWindowControl.xaml.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace CFlow
{
    using GraphX.Controls;
    using GraphX.PCL.Common.Enums;
    using GraphX.PCL.Logic.Algorithms.LayoutAlgorithms;
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
           
            InitializeComponent();
            ZoomControl.SetViewFinderVisibility(Zoomctrl, Visibility.Visible);


            Loaded += button1_Click;
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
            var Model = Load();

            //This property sets layout algorithm that will be used to calculate vertices positions
            //Different algorithms uses different values and some of them uses edge Weight property.
            Model.LogicCore.DefaultLayoutAlgorithm = LayoutAlgorithmTypeEnum.KK;
            //Now we can set parameters for selected algorithm using AlgorithmFactory property. This property provides methods for
            //creating all available algorithms and algo parameters.
            Model.LogicCore.DefaultLayoutAlgorithmParams = Model.LogicCore.AlgorithmFactory.CreateLayoutParameters(LayoutAlgorithmTypeEnum.Circular);
            //Unfortunately to change algo parameters you need to specify params type which is different for every algorithm.
            //((KKLayoutParameters)Model.LogicCore.DefaultLayoutAlgorithmParams).MaxIterations = 100;

            //This property sets vertex overlap removal algorithm.
            //Such algorithms help to arrange vertices in the layout so no one overlaps each other.
            Model.LogicCore.DefaultOverlapRemovalAlgorithm = OverlapRemovalAlgorithmTypeEnum.FSA;
            //Default parameters are created automaticaly when new default algorithm is set and previous params were NULL
            Model.LogicCore.DefaultOverlapRemovalAlgorithmParams.HorizontalGap = 50;
            Model.LogicCore.DefaultOverlapRemovalAlgorithmParams.VerticalGap = 50;

            //This property sets edge routing algorithm that is used to build route paths according to algorithm logic.
            //For ex., SimpleER algorithm will try to set edge paths around vertices so no edge will intersect any vertex.
            //Bundling algorithm will try to tie different edges that follows same direction to a single channel making complex graphs more appealing.
            Model.LogicCore.DefaultEdgeRoutingAlgorithm = EdgeRoutingAlgorithmTypeEnum.SimpleER;

            //This property sets async algorithms computation so methods like: Area.RelayoutGraph() and Area.GenerateGraph()
            //will run async with the UI thread. Completion of the specified methods can be catched by corresponding events:
            //Area.RelayoutFinished and Area.GenerateGraphFinished.
            Model.LogicCore.AsyncAlgorithmCompute = false;

            //Finally assign logic core to GraphArea object
            Area.LogicCore = Model.LogicCore;

            //Lets generate configured graph using pre-created data graph assigned to LogicCore object.
            //Optionaly we set first method param to True (True by default) so this method will automatically generate edges
            //  If you want to increase performance in cases where edges don't need to be drawn at first you can set it to False.
            //  You can also handle edge generation by calling manually Area.GenerateAllEdges() method.
            //Optionaly we set second param to True (True by default) so this method will automaticaly checks and assigns missing unique data ids
            //for edges and vertices in _dataGraph.
            //Note! Area.Graph property will be replaced by supplied _dataGraph object (if any).
            Area.GenerateGraph(true, true);

            /* 
             * After graph generation is finished you can apply some additional settings for newly created visual vertex and edge controls
             * (VertexControl and EdgeControl classes).
             * 
             */

            //This method sets the dash style for edges. It is applied to all edges in Area.EdgesList. You can also set dash property for
            //each edge individually using EdgeControl.DashStyle property.
            //For ex.: Area.EdgesList[0].DashStyle = GraphX.EdgeDashStyle.Dash;
            Area.SetEdgesDashStyle(EdgeDashStyle.Dash);

            //This method sets edges arrows visibility. It is also applied to all edges in Area.EdgesList. You can also set property for
            //each edge individually using property, for ex: Area.EdgesList[0].ShowArrows = true;
            Area.ShowAllEdgesArrows(true);

            //This method sets edges labels visibility. It is also applied to all edges in Area.EdgesList. You can also set property for
            //each edge individually using property, for ex: Area.EdgesList[0].ShowLabel = true;
            Area.ShowAllEdgesLabels(true);

            Zoomctrl.ZoomToFill();

        }

        private FunctionViewModel Model { get; set; }

        private FunctionViewModel Load()
        {
            var package = (CFlowPackage)ReverseFunctionWindowCommand.Instance.ServiceProvider;
            var projects = VSHelper.GetProjects(package.Dte);
            if (!projects.Any())
                return null;

            var project = projects.First();
            var files = VSHelper.GetFiles(project);
            files = files.Where(f => Path.GetExtension(f) != ".ld").ToList();


            var treeRunner = new Runner.TreeRunner(package.CFlow, files);
            var treeOutput = treeRunner.Run();
            var treeResults = Parser.CFlowParser.Parse(new List<string>(treeOutput.Split(new string[] { Environment.NewLine }, StringSplitOptions.None)));

            return new FunctionViewModel(treeResults);

        }
    }
}