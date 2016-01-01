using System;
using System.Collections.Generic;

namespace CFlow.Runner
{
    public class XrefRunner : AbstractRunner
    {
        public XrefRunner(string cflow, List<string> files) : base(cflow, files)
        {

        }
        protected override string GetArguments(string tempfile)
        {
            var arguments = new List<string>
            {
                "--xref",
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
