using GraphX.PCL.Common.Models;

namespace CFlow
{
    public class FunctionEdge : EdgeBase<FunctionVertex>
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
