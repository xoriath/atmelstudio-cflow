using CFlow.Parser;

namespace CFlow
{
    public class FunctionVertex
    {
        public FunctionNode FunctionNode { get; private set; }

        public string Name { get; private set; }
        public FunctionVertex(FunctionNode f)
        {
            FunctionNode = f;

            Name = $"{ f.Function.Name } { f.Function.Signature }";
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
