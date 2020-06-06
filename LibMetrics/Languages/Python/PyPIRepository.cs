using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace LibMetrics.Languages.Python
{
  public class PyPIRepository: IPackageRepository
  {
    private IDictionary<string, IList<VersionInfo>> _packages =
      new Dictionary<string, IList<VersionInfo>>();

    private IList<VersionInfo> GetReleaseHistory(string name)
    {
      if (_packages.ContainsKey(name))
      {
        return _packages[name];
      }

      var url = $"https://pypi.org/p/{name}";
      var web = new HtmlWeb();
      var doc = web.Load(url);

      var versions = new List<VersionInfo>();

      var releaseNodes = doc.DocumentNode.Descendants("div").
        Where(div => div.HasClass("release"));

      foreach (var releaseNode in releaseNodes)
      {
        var versionNode = releaseNode.Descendants("p").
          First(p => p.HasClass("release__version"));
        var rawVersion = Regex.Replace(versionNode.InnerText, @"\s+", " ").
          Trim();

        var versionSplits = rawVersion.Split(' ');

        if (versionSplits.Length > 1)
        {
          continue;
        }
        var version = versionSplits[0];

        var dateNode = releaseNode.Descendants("time").First();
        var versionDate = DateTime.Parse(dateNode.Attributes["datetime"].Value).
          ToUniversalTime();

        versions.Add(new VersionInfo(version, versionDate));
      }

      _packages[name] = versions;
      return versions;
    }

    public VersionInfo LatestAsOf(DateTime date, string name)
    {
      return GetReleaseHistory(name).OrderByDescending(v => v).
        First(v => date >= v.DatePublished);
    }

    public VersionInfo VersionInfo(string name, string version)
    {
      return GetReleaseHistory(name).First(v => v.Version == version);
    }

    public VersionInfo Latest(string name, string thatMatches, DateTime asOf)
    {
      var expression = VersionMatcher.Create(thatMatches);
      return GetReleaseHistory(name).OrderByDescending(v => v).
        Where(v => v.DatePublished <= asOf).
        First(v => expression.DoesMatch(v));
    }
  }
}
