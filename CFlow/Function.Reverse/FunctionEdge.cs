﻿using QuickGraph;

namespace CFlow
{
    public class FunctionEdge : Edge<FunctionVertex>
    {
        public string ID
        {
            get; private set;
        }
        public FunctionEdge(string id, FunctionVertex source, FunctionVertex target) : base(source, target)
        {
            ID = id;
        }

        public override string ToString()
        {
            return ID;
        }
    }
}