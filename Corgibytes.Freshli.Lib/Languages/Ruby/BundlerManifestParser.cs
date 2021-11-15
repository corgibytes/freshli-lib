using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Corgibytes.Freshli.Lib.Languages.Ruby
{
    public class BundlerManifestParser : IManifestParser
    {
        public bool UsesExactMatches => true;

        public IEnumerable<PackageInfo> Parse(Stream contentsStream)
        {
            var reader = new StreamReader(contentsStream);
            var gemSectionLine = reader.ReadLine();
            while (gemSectionLine != null && gemSectionLine != "GEM")
            {
                gemSectionLine = reader.ReadLine();
            }

            if (gemSectionLine != "GEM")
            {
                throw new FormatException("Unrecognized format for Gemfile.lock.");
            }

            var line = reader.ReadLine();
            while (line != null && line != "  specs:")
            {
                line = reader.ReadLine();
            }

            line = reader.ReadLine();
            while (line != null && line.Trim() != "")
            {
                var expression = new Regex(
                          @"^    (?<name>[\w\d_\-\.]+) \((?<version>[^\)]+)\)"
                        );
                var match = expression.Match(line);
                if (match.Success)
                {
                    yield return new PackageInfo(
                      match.Groups["name"].Value,
                      match.Groups["version"].Value
                    );
                }

                line = reader.ReadLine();
            }
        }
    }
}
