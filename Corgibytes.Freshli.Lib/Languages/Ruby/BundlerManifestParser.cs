using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Corgibytes.Freshli.Lib.Languages.Ruby
{
    public class BundlerManifestParser : IManifestParser
    {
        private Dictionary<Stream, int> parseCounts = new Dictionary<Stream, int>();
        public bool UsesExactMatches => true;

        public IEnumerable<PackageInfo> Parse(Stream contentsStream)
        {
            if (contentsStream.CanSeek)
            {
                // HACK: This helps resolve some issues that can pop up if parse is called
                // more than once with the same `Stream` object. That is something
                // that can happen if the `IEnumerable<PackageInfo>` instance that
                // this method is building is iterated more than once. _Ideally_
                // such an iteration would only ever need to happen once, so this
                // should be something that we're diligent enough about avoiding.
                contentsStream.Seek(0, SeekOrigin.Begin);
            }

            if (!parseCounts.ContainsKey(contentsStream))
            {
                parseCounts[contentsStream] = 0;
            }
            else
            {
                // TODO: Log a warning to help us detect the situations when
                // the hack mentioned above is needed.
                parseCounts[contentsStream] += 1;
            }

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
