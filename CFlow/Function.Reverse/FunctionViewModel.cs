using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using CFlow.Parser;
using System.Linq;

namespace CFlow
{
    class FunctionViewModel : INotifyPropertyChanged
    {
        private FunctionNode FunctionRootNode;

        public FunctionViewModel(FunctionNode functionRootNode)
        {
            FunctionRootNode = functionRootNode;
            Graph = new FunctionGraph(true);

            //AddTree();
            

            //Pick a default Layout Algorithm Type
            LayoutAlgorithmType = "LinLog";
        }

        private void AddTree()
        {
            var vertex = new FunctionVertex(FunctionRootNode);
            Graph.AddVertex(vertex);

            //AddSubTree(vertex);
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

        private void AddAlgorithmTypes()
        {
            layoutAlgorithmTypes.Add("BoundedFR");
            layoutAlgorithmTypes.Add("Circular");
            layoutAlgorithmTypes.Add("CompoundFDP");
            layoutAlgorithmTypes.Add("EfficientSugiyama");
            layoutAlgorithmTypes.Add("FR");
            layoutAlgorithmTypes.Add("ISOM");
            layoutAlgorithmTypes.Add("KK");
            layoutAlgorithmTypes.Add("LinLog");
            layoutAlgorithmTypes.Add("Tree");
        }

        private FunctionEdge AddNewGraphEdge(FunctionVertex from, FunctionVertex to)
        {
            var id = $" { from.Name } - { to.Name }";

            var edge = new FunctionEdge(id, from, to);
            Graph.AddEdge(edge);

            return edge;
        }


        private List<string> layoutAlgorithmTypes = new List<string>();
        public List<string> LayoutAlgorithmTypes
        {
            get { return layoutAlgorithmTypes; }
        }

        private string layoutAlgorithmType;
        public string LayoutAlgorithmType
        {
            get { return layoutAlgorithmType; }
            set
            {
                layoutAlgorithmType = value;
                NotifyPropertyChanged();
            }
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


        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName]string name = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

    }
}
