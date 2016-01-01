using System.Collections.Generic;
using System.Diagnostics;

namespace CFlow.Parser
{
    [DebuggerDisplay("{Function.Name}")]
    public class FunctionNode
    {
        public Function Function { get; private set; }

        private IList<FunctionNode> children;
        public IList<FunctionNode> Children {
            get { return children; }
            set { if (null == value)
                    children = new List<FunctionNode>();
                  else
                    children = value; }
        }

        public FunctionNode(Function function, IList<FunctionNode> children)
        {
            Function = function;
            Children = children;
        }
    }
    [DebuggerDisplay("{Function.Name}")]
    public class FunctionReverseNode
    {
        public Function Function { get; private set; }

        private IList<FunctionReverseNode> parents;
        public IList<FunctionReverseNode> Parents
        {
            get { return parents; }
            set
            {
                if (null == value)
                    parents = new List<FunctionReverseNode>();
                else
                    parents = value;
            }
        }
        
        public FunctionReverseNode(Function function, IList<FunctionReverseNode> parent = null)
        {
            Function = function;
            Parents = parents;
        }
    }
}
