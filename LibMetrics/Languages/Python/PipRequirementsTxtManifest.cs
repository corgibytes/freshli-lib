using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace LibMetrics.Languages.Python
{
  public class PipRequirementsTxtManifest: IManifest
  {
    private IDictionary<string, PackageInfo> _packages = new Dictionary<string, PackageInfo>();

    public IEnumerator<PackageInfo> GetEnumerator()
    {
      return _packages.Values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    public int Count => _packages.Count;

    public void Add(string packageName, string packageVersion)
    {
      _packages[packageName] = new PackageInfo(packageName, packageVersion);
    }

    public void Parse(string contents)
    {
      var versionMatcher = new Regex(@"^((\w|\d)+)(.*)");
      var reader = new StringReader(contents);
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
            version = match.Groups[3].Value;
          }

          Add(packageName, version);
        }
        line = reader.ReadLine();
      }
    }

    public PackageInfo this[string packageName] => _packages[packageName];
    public bool UsesExactMatches => false;
  }
}
