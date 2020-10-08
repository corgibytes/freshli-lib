using System;
using System.Collections.Generic;
using System.Linq;
using Freshli.Exceptions;
using HtmlAgilityPack;

namespace Freshli.Languages.Ruby {
  public class RubyGemsRepository : IPackageRepository {

    private IDictionary<string, IList<IVersionInfo>> _packages =
      new Dictionary<string, IList<IVersionInfo>>();

    private IEnumerable<IVersionInfo> GetReleaseHistory(
      string name,
      bool includePreReleaseVersions)
    {
      try {
        var keySuffix =
          includePreReleaseVersions ? "-with-pre-releases" : "";
        var key = $"{name}{keySuffix}";
        if (_packages.ContainsKey(key)) {
          return _packages[key];
        }

        var url = $"https://rubygems.org/gems/{name}/versions";
        var web = new HtmlWeb();
        var document = web.Load(url);
        var versions = new List<IVersionInfo>();

        var releaseNodes = document.DocumentNode.Descendants("li").
          Where(li => li.HasClass("gem__version-wrap"));

        foreach (var releaseNode in releaseNodes) {
          var version = releaseNode.Descendants("a").
            First(a => a.HasClass("t-list__item")).InnerText;

          var rawDate = releaseNode.Descendants("small").First().InnerText.
            Replace("- ", "");
          var versionDate = DateTime.ParseExact(rawDate, "MMMM dd, yyyy", null);

          var versionInfo = new RubyGemsVersionInfo(version, versionDate);
          if ((!versionInfo.IsPreRelease || includePreReleaseVersions) &&
            !IsReleasePlatformSpecific(releaseNode)) {
            versions.Add(versionInfo);
          }
        }
        _packages[key] = versions;
        return versions;

      } catch (Exception e) {
        throw new DependencyNotFoundException(name, e);
      }
    }

    public IVersionInfo Latest(
      string name,
      DateTime asOf,
      bool includePreReleases)
    {
      try {
        return GetReleaseHistory(name, includePreReleases).
          OrderByDescending(v => v).
          First(v => asOf >= v.DatePublished);
      } catch (Exception e) {
        throw new LatestVersionNotFoundException(name, asOf, e);
      }
    }

    public IVersionInfo VersionInfo(string name, string version) {
      try {
        return GetReleaseHistory(name, includePreReleaseVersions: true).
          First(v => v.Version == version);
      } catch (Exception e) {
        throw new VersionNotFoundException(name, version, e);
      }
    }

    public IVersionInfo Latest(
      string name,
      DateTime asOf,
      string thatMatches) {
      throw new NotImplementedException();
    }

    public List<IVersionInfo> VersionsBetween(
      string name,
      DateTime asOf,
      IVersionInfo earlierVersion,
      IVersionInfo laterVersion,
      bool includePreReleases)
    {
      try {
        return GetReleaseHistory(name , includePreReleases).
          OrderByDescending(v => v).
          Where(v => asOf >= v.DatePublished).
          Where(predicate: v => v.CompareTo(earlierVersion) == 1).
          Where(predicate: v => v.CompareTo(laterVersion) == -1).ToList();
      }
      catch (Exception e) {
        throw new VersionsBetweenNotFoundException(
          name, earlierVersion.Version, laterVersion.Version, e);
      }
    }

    private static bool IsReleasePlatformSpecific(HtmlNode node) {
      var platformSpecific = false;
      foreach (var span in node.Descendants("span")) {
        foreach (var className in span.GetClasses()) {
          if (className == "platform") {
            platformSpecific = true;
          }
        }
      }
      return platformSpecific;
    }
  }
}
