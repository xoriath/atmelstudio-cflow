
using CFlow.Parser;
using QuickGraph;

namespace CFlow
{
    public class FunctionGraph : BidirectionalGraph<FunctionVertex, FunctionEdge>
    {
        public FunctionGraph() { }

        public FunctionGraph(bool allowParallelEdges) : base(allowParallelEdges) { }

        public FunctionGraph(bool allowParallelEdges, int vertexCapacity) : base(allowParallelEdges, vertexCapacity) { }
    }
}
