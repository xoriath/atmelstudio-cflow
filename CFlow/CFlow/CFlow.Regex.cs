using System.Text.RegularExpressions;

namespace CFlow.Parser
{
    internal static class Regexes
    {
        public static readonly Regex LineWithFileInfo = new Regex(@"(^[\s]*)([\S^:]+): ([^,]*), <(.*) (\d+)>", RegexOptions.Compiled);
        public static readonly Regex LineWithoutFileInfo = new Regex(@"(^[\s]*)([\S^:]+): ([^,]*), <>", RegexOptions.Compiled);
        public static readonly Regex LineWithoutFileAndSignatureInfo = new Regex(@"(^[\s]*)([\S^:]+): <>", RegexOptions.Compiled);


        public static readonly Regex LineWithXrefAndSignature = new Regex(@"(\S+) ( |\*) (.+):(\d+) (.+)", RegexOptions.Compiled);
        public static readonly Regex LineWithXref = new Regex(@"(\S+) ( |\*) (.+):(\d+)", RegexOptions.Compiled);
    }
}
