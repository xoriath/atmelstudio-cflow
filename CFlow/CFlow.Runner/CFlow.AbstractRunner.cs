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

        private string WorkingDir;

        public List<string> Files { get; private set; }

        public AbstractRunner(string cflow, List<string> files)
        {
            CFlow = cflow;
            Files = files;

            var MatchingChars =
                from len in Enumerable.Range(0, Files.Min(s => s.Length)).Reverse()
                let possibleMatch = Files.First().Substring(0, len)
                where Files.All(f => f.StartsWith(possibleMatch))
                select possibleMatch;

            WorkingDir = Path.GetDirectoryName(MatchingChars.First());

            Files = Files.Select(f => f.Remove(0, WorkingDir.Length + 1)).ToList();
        }
        
        public string Run()
        {
            var tempFile = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            var startInfo = new ProcessStartInfo
            {
                Arguments = GetArguments(tempFile),
                FileName = CFlow,
                UseShellExecute = false,
                CreateNoWindow = false,
                WorkingDirectory = WorkingDir
            };

            var process = new Process { StartInfo = startInfo };
            process.Start();

            process.WaitForExit();

            if (process.ExitCode != 0)
                return string.Empty;

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
