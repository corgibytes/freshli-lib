using System.IO;
using System.Text.RegularExpressions;

namespace Freshli.Languages.Python {
  public class PipRequirementsTxtManifest : AbstractManifest {
    public override void Parse(string contents) {
      Clear();

      var versionMatcher = new Regex(
        @"^((\w|\d|\.|-)+)((?:(?:~=)|(?:===?)|(?:!=)|(?:<=?)|(?:>=?))?.*)"
      );
      var reader = new StringReader(contents);
      var line = reader.ReadLine();
      while (line != null) {
        var matches = versionMatcher.Matches(line);
        if (matches.Count > 0) {
          var match = matches[0];
          var packageName = match.Groups[1].Value;
          var version = "";
          if (match.Groups.Count > 3) {
            version = match.Groups[3].Value.Replace(" ", "");
          }

          Add(packageName, version);
        }

        line = reader.ReadLine();
      }
    }

    public override bool UsesExactMatches => false;
  }
}
