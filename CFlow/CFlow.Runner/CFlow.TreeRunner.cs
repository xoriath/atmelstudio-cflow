using System;
using System.Collections.Generic;

namespace CFlow.Runner
{
    public class TreeRunner : AbstractRunner
    {
        public TreeRunner(string cflow, List<string> files) : base(cflow, files)
        {

        }
        protected override string GetArguments(string tempfile)
        {
            var arguments = new List<string>
            {
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
