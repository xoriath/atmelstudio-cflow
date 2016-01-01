
using System.Collections.Generic;
using System.Linq;

namespace CFlow.Parser
{
    public static class CFlowParser
    {
        public static FunctionNode Parse(IList<string> graph)
        {
            var nodes = graph.Select(x => ParseLine(x)).Where(x => x != null);

            return BuildGraph(nodes.ToList());
        }

        private static FunctionNode BuildGraph(List<ParseFragment> nodes)
        {
            var root = nodes.First().Node;
            foreach (var node in nodes.Skip(1))
            {
                var level = node.Indent;
                var iterator = root;
                while (level > 1)
                {
                    level--;
                    iterator = iterator.Children.Last();
                }

                iterator.Children.Add(node.Node);
            }

            return root;
        }

       
        internal static ParseFragment ParseLine(string line)
        {
            // main: int (void), <c:\Users\Morten\Documents\Atmel Studio\7.0\GccApplication1\GccApplication1\main.cpp 14>
            var leadingWhiteSpace = line.TakeWhile(c => char.IsWhiteSpace(c)).Count();
            var leadingTabs = leadingWhiteSpace / 4;

            if (Regexes.LineWithFileInfo.IsMatch(line))
                return ParseLineWithFileInfo(line, leadingTabs);
            else if (Regexes.LineWithoutFileInfo.IsMatch(line))
                return ParseLineWithoutFileInfo(line, leadingTabs);
            else if (Regexes.LineWithoutFileAndSignatureInfo.IsMatch(line))
                return ParseLineWithoutFileAndSignatureInfo(line, leadingTabs);
            else
                return null;


        }

        private static ParseFragment ParseLineWithoutFileAndSignatureInfo(string line, int leadingTabs)
        {
            var match = Regexes.LineWithoutFileAndSignatureInfo.Match(line);

            var leadingSpace = match.Groups[1].Value;
            var name = match.Groups[2].Value;
            var signature = string.Empty;
            var filename = string.Empty;
            var lineNumber = "0";

            return new ParseFragment
            {
                Node = new FunctionNode(Function.Create(name, signature, filename, lineNumber), null),
                Indent = leadingTabs
            };
        }

        private static ParseFragment ParseLineWithoutFileInfo(string line, int leadingTabs)
        {
            var match = Regexes.LineWithoutFileInfo.Match(line);

            var leadingSpace = match.Groups[1].Value;
            var name = match.Groups[2].Value;
            var signature = match.Groups[3].Value;
            var filename = string.Empty;
            var lineNumber = "0";

            return new ParseFragment
            {
                Node = new FunctionNode(Function.Create(name, signature, filename, lineNumber), null),
                Indent = leadingTabs
            };
        }

        private static ParseFragment ParseLineWithFileInfo(string line, int leadingTabs)
        {
            var match = Regexes.LineWithFileInfo.Match(line);

            var leadingSpace = match.Groups[1].Value;
            var name = match.Groups[2].Value;
            var signature = match.Groups[3].Value;
            var filename = match.Groups[4].Value;
            var lineNumber = match.Groups[5].Value;

            return new ParseFragment
            {
                Node = new FunctionNode(Function.Create(name, signature, filename, lineNumber), null),
                Indent = leadingTabs
            };
        }
    }
}
