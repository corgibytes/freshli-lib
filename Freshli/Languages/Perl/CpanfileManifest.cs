using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Freshli.Languages.Perl {
  public class CpanfileManifest : AbstractManifest {
    public override void Parse(string contents) {
      Clear();
      var reader = new StringReader(contents);
      
      var lineExpression = new Regex("requires '([^']*)', ?'([^']*)'.*");
      
      var line = reader.ReadLine();
      while (line != null) {
        line = line.Trim();

        var match = lineExpression.Match(line);
        if (match.Success) {
          var packageName = match.Groups["1"].Value;
          var versionExpression = match.Groups["2"].Value;

          Add(packageName, versionExpression);
        }

        line = reader.ReadLine();
      }
    }

    public override bool UsesExactMatches => false;
  }
}
