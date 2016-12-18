using System;
using System.Collections.Generic;

namespace CFlow.Parser
{
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

        public override string ToString()
        {
            return $"{ Function } => { Children.Count } children";
        }

        public override int GetHashCode()
        {
            return Function.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Function.Equals(obj);
        }
    }

    public class FunctionNodeEqualityComparer : IEqualityComparer<FunctionNode>
    {
        bool IEqualityComparer<FunctionNode>.Equals(FunctionNode x, FunctionNode y)
        {
            return x.Function.Equals(y.Function);
        }

        int IEqualityComparer<FunctionNode>.GetHashCode(FunctionNode obj)
        {
            return obj.Function.GetHashCode();
        }
    }

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

        public override string ToString()
        {
            return $"{ Function } => { Parents.Count } parents";
        }
    }
}
