using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using CFlow.Parser;
using System.Linq;
using GraphX.PCL.Common.Enums;
using GraphX.PCL.Logic.Algorithms.LayoutAlgorithms;

namespace CFlow
{
    class FunctionViewModel : INotifyPropertyChanged
    {
        private FunctionNode FunctionRootNode;

        public FunctionViewModel(FunctionNode functionRootNode)
        {
            FunctionRootNode = functionRootNode;
            Graph = new FunctionGraph(true);

            AddTree();

            LogicCore = new LogicCore { Graph = Graph };

            LogicCore.DefaultLayoutAlgorithm = LayoutAlgorithmTypeEnum.KK;

            LogicCore.DefaultLayoutAlgorithmParams = LogicCore.AlgorithmFactory.CreateLayoutParameters(LayoutAlgorithmTypeEnum.KK);

            ((KKLayoutParameters)LogicCore.DefaultLayoutAlgorithmParams).MaxIterations = 100;

            LogicCore.DefaultOverlapRemovalAlgorithm = OverlapRemovalAlgorithmTypeEnum.FSA;

            LogicCore.DefaultOverlapRemovalAlgorithmParams.HorizontalGap = 50;
            LogicCore.DefaultOverlapRemovalAlgorithmParams.VerticalGap = 50;

            LogicCore.DefaultEdgeRoutingAlgorithm = EdgeRoutingAlgorithmTypeEnum.SimpleER;

            LogicCore.AsyncAlgorithmCompute = false;
        }

        private void AddTree()
        {
            var vertex = new FunctionVertex(FunctionRootNode);
            Graph.AddVertex(vertex);

            AddSubTree(vertex);
        }

        private void AddSubTree(FunctionVertex vertex)
        {
            foreach (var c in vertex.FunctionNode.Children)
            {
                var childVertex = new FunctionVertex(c);
                Graph.AddVertex(childVertex);

                AddNewGraphEdge(vertex, childVertex);

                AddSubTree(childVertex);
            }
        }
        
        private FunctionEdge AddNewGraphEdge(FunctionVertex from, FunctionVertex to)
        {
            var id = $" { from.Name } - { to.Name }";

            var edge = new FunctionEdge(id, from, to);
            Graph.AddEdge(edge);

            return edge;
        }

        
        private FunctionGraph graph;
        public FunctionGraph Graph
        {
            get { return graph; }
            set
            {
                graph = value;
                NotifyPropertyChanged();
            }
        }

        private LogicCore logicCore;
        public LogicCore LogicCore
        {
            get { return logicCore; }
            set
            {
                logicCore = value;
                NotifyPropertyChanged();
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName]string name = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

    }
}
