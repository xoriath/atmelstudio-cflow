using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.IO;

namespace CFlow.Runner
{
    public abstract class AbstractRunner
    {
        private string CFlow;
        public List<string> Files { get; private set; }

        public AbstractRunner(string cflow, List<string> files)
        {
            CFlow = cflow;
            Files = files;
        }

        public string Run()
        {
            var tempFile = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            var startInfo = new ProcessStartInfo
            {
                Arguments = GetArguments(tempFile),
                FileName = CFlow,
                UseShellExecute = false,
                CreateNoWindow = true,
            };

            var process = new Process { StartInfo = startInfo };
            process.Start();

            process.WaitForExit();

            var result = File.ReadAllText(tempFile);
            File.Delete(tempFile);

            return result;
        }

        abstract protected string GetArguments(string tempfile);
        

        protected IEnumerable<string> GetFileArgument()
        {
            return Files.Select(f => $"\"{ f }\"");
        }
    }


}
