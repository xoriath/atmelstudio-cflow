
using System.Collections.Generic;
using System.Linq;

namespace CFlow.Parser
{
    public class CFlowXrefParser
    {
        public static IList<XrefFunction> Parse(IList<string> list)
        {
            var lines = list.Select(l => ParseLine(l)).Where(l => l != null).ToList();

            var primaries = lines.Where(f => f.IsPrimary).ToList();
            var secondaries = lines.Where(f => !f.IsPrimary).ToList();

            return BuildXrefs(primaries, secondaries);
        }

        public static IDictionary<string, List<XrefFunction>> ParseUnbound(IList<string> list)
        {
            var lines = list.Select(l => ParseLine(l)).Where(l => l != null).ToList();

            var primaries = lines.Where(f => f.IsPrimary).ToList();
            var secondaries = lines.Where(f => !f.IsPrimary).ToList();

            var tree = BuildXrefs(primaries, secondaries);
            var boundSecondaries = tree.SelectMany(r => r.References);

            var unboundSecondaries = secondaries.Except(boundSecondaries);

            Dictionary<string, List<XrefFunction>> ret = new Dictionary<string, List<XrefFunction>>();
            foreach (var secondary in unboundSecondaries)
            {
                if (!ret.ContainsKey(secondary.Name))
                    ret[secondary.Name] = new List<XrefFunction>();
                ret[secondary.Name].Add(secondary);
            }

            return ret;
        }

        private static IList<XrefFunction> BuildXrefs(List<XrefFunction> primaries, List<XrefFunction> secondaries)
        {
            foreach(var primary in primaries)
            {
                foreach(var secondary in secondaries)
                {
                    if (secondary.Name.Equals(primary.Name))
                        primary.References.Add(secondary);
                }
            }

            return primaries;
        }

        private static XrefFunction ParseLine(string line)
        {
            if (Regexes.LineWithXrefAndSignature.IsMatch(line))
                return ParseLineWithXrefAndSignature(line);
            else if (Regexes.LineWithXref.IsMatch(line))
                return ParseLineWithXref(line);
            else
                return null;

        }

        private static XrefFunction ParseLineWithXref(string line)
        {
            var match = Regexes.LineWithXref.Match(line);

            var name = match.Groups[1].Value;
            var primary = match.Groups[2].Value.Trim().Equals("*");
            var file = match.Groups[3].Value;
            var lineNumber = int.Parse(match.Groups[4].Value);

            return new XrefFunction(name, file, lineNumber, primary);
        }

        private static XrefFunction ParseLineWithXrefAndSignature(string line)
        {
            var match = Regexes.LineWithXrefAndSignature.Match(line);

            var name = match.Groups[1].Value;
            var primary = match.Groups[2].Value.Trim().Equals("*");
            var file = match.Groups[3].Value;
            var lineNumber = int.Parse(match.Groups[4].Value);
            var signature = match.Groups[5].Value;
            
            return new XrefFunction(name, file, lineNumber, primary, signature);
        }
    }
}
