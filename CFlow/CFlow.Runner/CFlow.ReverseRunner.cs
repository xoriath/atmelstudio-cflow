using System.Collections.Generic;

namespace CFlow.Runner
{
    class ReverseRunner : AbstractRunner
    {
        public ReverseRunner(string cflow, List<string> files) : base(cflow, files)
        {
            
        }

        protected override string GetArguments(string tempfile)
        {
            var arguments = new List<string>
            {
                "--reverse",
                "--brief",
                "--format=posix",
                "--no-brief",
                "--no-number",
                $"--output=\"{ tempfile }\""
            };

            arguments.AddRange(GetFileArgument());

            return string.Join(" ", arguments);
        }
    }
}
