using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Freshli.Languages.Ruby {
  public class BundlerManifest : AbstractManifest {
    public override void Parse(string contents) {
      Clear();

      var reader = new StringReader(contents);
      var gemSectionLine = reader.ReadLine();
      while (gemSectionLine != null && gemSectionLine != "GEM") {
        gemSectionLine = reader.ReadLine();
      }

      if (gemSectionLine != "GEM") {
        throw new FormatException("Unrecognized format for Gemfile.lock.");
      }

      var line = reader.ReadLine();
      while (line != null && line != "  specs:") {
        line = reader.ReadLine();
      }

      line = reader.ReadLine();
      while (line != null && line.Trim() != "") {
        line = ProcessLine(reader, line);
      }
    }

    private string ProcessLine(StringReader reader, string line) {
      var expression = new Regex(
                @"^    (?<name>[\w\d_\-\.]+) \((?<version>[^\)]+)\)"
              );
      var match = expression.Match(line);
      if (match.Success) {
        Add(
          match.Groups["name"].Value, 
          match.Groups["version"].Value
        );
      }

      line = reader.ReadLine();
      return line;
    }

    public override bool UsesExactMatches => true;
  }
}
