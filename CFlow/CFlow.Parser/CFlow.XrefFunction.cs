using System.Collections.Generic;

namespace CFlow.Parser
{
    public class XrefFunction
    {
        public string Name { get; private set; }
        public bool IsPrimary { get; private set; }
        public string File { get; private set; }
        public int Line { get; private set; }
        public string Signature { get; private set; }
        public IList<XrefFunction> References { get; set; } = new List<XrefFunction>();

        public XrefFunction(string name, string file, int line, bool primary, string signature = null)
        {
            Name = name;
            IsPrimary = primary;
            File = file;
            Line = line;
            Signature = signature ?? string.Empty;
        }

        public override string ToString()
        {
            return $"{ Name }: { Signature } <{ File }>:{ Line } => { References.Count } references";
        }
    }
}