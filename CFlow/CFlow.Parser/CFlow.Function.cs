namespace CFlow.Parser
{
    public class Function
    {
        public string Name { get; private set; }

        public string Signature { get; private set; }

        public string File { get; private set; }

        public uint Line { get; private set; }

        public static Function Create(string name, string signature, string file, uint line)
        {
            var f = new Function();
            f.Initialize(name, signature, file, line);
            return f;
        }

        public static Function Create(string name, string signature, string file, string line)
        {
            uint lineNo;
            if (!uint.TryParse(line, out lineNo))
                return null;

            return Create(name, signature, file, lineNo);
        }

        private void Initialize(string name, string signature, string file, uint line)
        {
            Name = name;
            Signature = signature;
            File = file;
            Line = line;
        }
    }
}
