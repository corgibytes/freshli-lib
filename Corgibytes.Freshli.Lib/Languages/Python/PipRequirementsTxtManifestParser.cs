using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Corgibytes.Freshli.Lib.Languages.Python
{
    public class PipRequirementsTxtManifestParser : IManifestParser
    {
        public bool UsesExactMatches => false;

        public IEnumerable<PackageInfo> Parse(Stream contentsStream)
        {
            var versionMatcher = new Regex(
              @"^((\w|\d|\.|-)+)((?:(?:~=)|(?:===?)|(?:!=)|(?:<=?)|(?:>=?))?.*)"
            );
            var reader = new StreamReader(contentsStream);
            var line = reader.ReadLine();
            while (line != null)
            {
                var matches = versionMatcher.Matches(line);
                if (matches.Count > 0)
                {
                    var match = matches[0];
                    var packageName = match.Groups[1].Value;
                    var version = "";
                    if (match.Groups.Count > 3)
                    {
                        version = match.Groups[3].Value.Replace(" ", "");
                    }

                    yield return new PackageInfo(packageName, version);
                }

                line = reader.ReadLine();
            }
        }
    }
}
