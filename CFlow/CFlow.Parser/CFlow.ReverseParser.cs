
using System;
using System.Collections.Generic;
using System.Linq;

namespace CFlow.Parser
{
    public static class CFlowReverseParser
    {
        public static IList<FunctionReverseNode> Parse(IList<string> graph)
        {
            var nodes = graph.Select(x => CFlowParser.ParseLine(x)).Where(x => x != null);

            return BuildGraph(nodes.ToList());
        }

        private static IList<FunctionReverseNode> BuildGraph(List<ParseFragment> nodes)
        {
            var res = new List<FunctionReverseNode>();

            for (int i = 0; i < nodes.Count; ++i)
            {
                if (nodes[i].Indent == 0)
                    res.Add(BuildTree(i, nodes));
            }

            return res;
        }

        // Create parent graph by doing recursive depth-first-search
        private static FunctionReverseNode BuildTree(int i, List<ParseFragment> nodes)
        {
            return BuildSubTree(i, nodes);
        }
       
        private static FunctionReverseNode BuildSubTree(int i, List<ParseFragment> nodes, int level = 0)
        {
            var root = new FunctionReverseNode(nodes[i].Node.Function);

            for (int j = i + 1; j < nodes.Count; ++j)
            {
                if (nodes[j].Indent == level + 1)
                    root.Parents.Add(BuildSubTree(j, nodes, level + 1));
                else
                    break;
            }

            return root;
        }
    }
}
