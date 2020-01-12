using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace LibMetrics
{
  public class BundlerManifest : IEnumerable<PackageInfo>
  {
    private List<PackageInfo> _list;

    public BundlerManifest()
    {
      _list = new List<PackageInfo>();
    }

    public int Count => _list.Count;

    public void Add(string packageName, string packageVersion)
    {
      _list.Add(new PackageInfo(packageName, packageVersion));
    }

    public IEnumerator<PackageInfo> GetEnumerator()
    {
      return _list.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    public void Parse(string contents)
    {
      _list.Clear();

      var reader = new StringReader(contents);
      var firstLine = reader.ReadLine();
      if (firstLine != "GEM")
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
          @"^    (?<name>[\w\d_\-\.]+) \((?<version>[^\)]+)\)");
        var match = expression.Match(line);
        if (match.Success)
        {
          var packageName = match.Groups["name"].Value;
          var packageVersion = match.Groups["version"].Value;
          Add(packageName, packageVersion);
        }

        line = reader.ReadLine();
      }
    }

    public PackageInfo this[string packageName]
    {
      get { return _list.First(package => package.Name == packageName); }
    }
  }
}
