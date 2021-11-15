using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Corgibytes.Freshli.Lib.Languages.Perl
{
    public class CpanfileManifestParser : IManifestParser
    {
        public bool UsesExactMatches => false;


        public IEnumerable<PackageInfo> Parse(Stream contentsStream)
        {
            var reader = new StreamReader(contentsStream);

            var lineExpression = new Regex("requires '([^']*)', ?'([^']*)'.*");

            var line = reader.ReadLine();
            while (line != null)
            {
                line = line.Trim();

                var match = lineExpression.Match(line);
                if (match.Success)
                {
                    var packageName = match.Groups["1"].Value;
                    var versionExpression = match.Groups["2"].Value;

                    yield return new PackageInfo(packageName, versionExpression);
                }

                line = reader.ReadLine();
            }
        }
    }
}
