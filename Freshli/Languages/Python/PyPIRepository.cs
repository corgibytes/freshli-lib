using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Freshli.Exceptions;
using HtmlAgilityPack;

namespace Freshli.Languages.Python {
  public class PyPIRepository : IPackageRepository {
    private IDictionary<string, IList<IVersionInfo>> _packages =
      new Dictionary<string, IList<IVersionInfo>>();

    private IList<IVersionInfo> GetReleaseHistory(string name) {
      try {
        if (_packages.ContainsKey(name)) {
          return _packages[name];
        }

        var url = $"https://pypi.org/p/{name}";
        var web = new HtmlWeb();
        var doc = web.Load(url);

        var versions = new List<IVersionInfo>();

        var releaseNodes = doc.DocumentNode.Descendants("div").
          Where(div => div.HasClass("release"));

        foreach (var releaseNode in releaseNodes) {
          var versionNode = releaseNode.Descendants("p").
            First(p => p.HasClass("release__version"));
          var rawVersion = Regex.Replace(versionNode.InnerText, @"\s+", " ").
            Trim();

          var versionSplits = rawVersion.Split(' ');

          if (versionSplits.Length > 1) {
            continue;
          }

          var version = versionSplits[0];

          var dateNode = releaseNode.Descendants("time").First();
          var versionDate = DateTime.Parse(
            dateNode.Attributes["datetime"].Value).ToUniversalTime();

          versions.Add(new SemVerVersionInfo(version, versionDate));
        }

        _packages[name] = versions;
        return versions;
      }
      catch (Exception e) {
        throw new DependencyNotFoundException(name, e);
      }
    }

    //TODO: Update logic to utilize includePreReleases
    public IVersionInfo Latest(
      string name,
      DateTime asOf,
      bool includePreReleases)
    {
      try {
        return GetReleaseHistory(name).OrderByDescending(v => v).
          First(v => asOf >= v.DatePublished);
      }
      catch (VersionParseException) {
        throw;
      }
      catch (Exception e) {
        throw new LatestVersionNotFoundException(name, asOf, e);
      }
    }

    public IVersionInfo VersionInfo(string name, string version) {
      return GetReleaseHistory(name).First(v => v.Version == version);
    }

    public IVersionInfo Latest(string name, DateTime asOf, string thatMatches) {
      try {
        var expression = VersionMatcher.Create(thatMatches);
        return GetReleaseHistory(name).OrderByDescending(v => v).
          Where(v => v.DatePublished <= asOf).
          First(v => expression.DoesMatch(v));
      }
      catch (VersionParseException) {
        throw;
      }
      catch (Exception e) {
        throw new LatestVersionThatMatchesNotFoundException(
          name, asOf, thatMatches, e);
      }
    }

    //TODO: Update logic to utilize includePreReleases
    public List<IVersionInfo> VersionsBetween(
      string name,
      DateTime asOf,
      IVersionInfo earlierVersion,
      IVersionInfo laterVersion,
      bool includePreReleases)
    {
      try {
        return GetReleaseHistory(name).
          OrderByDescending(v => v).
          Where(v => v.DatePublished <= asOf).
          Where(predicate: v => v.CompareTo(earlierVersion) == 1).
          Where(predicate: v => v.CompareTo(laterVersion) == -1).ToList();
      }
      catch (Exception e) {
        throw new VersionsBetweenNotFoundException(
          name, earlierVersion.Version, laterVersion.Version, e);
      }
    }

  }
}
