using System;
using System.Collections.Generic;
using System.Linq;
using Freshli.Exceptions;
using HtmlAgilityPack;

namespace Freshli.Languages.Ruby {
  public class RubyGemsRepository : IPackageRepository {

    private IDictionary<string, IList<VersionInfo>> _packages =
      new Dictionary<string, IList<VersionInfo>>();

    private IList<VersionInfo> GetReleaseHistory(string name) {
      try {

        if (_packages.ContainsKey(name)) {
          return _packages[name];
        }

        var url = $"https://rubygems.org/gems/{name}/versions";
        var web = new HtmlWeb();
        var document = web.Load(url);
        var versions = new List<VersionInfo>();

        var releaseNodes = document.DocumentNode.Descendants("li").
          Where(li => li.HasClass("gem__version-wrap"));

        foreach (var releaseNode in releaseNodes) {
          var version = releaseNode.Descendants("a").
            First(a => a.HasClass("t-list__item")).InnerText;

          var rawDate = releaseNode.Descendants("small").First().InnerText.
            Replace("- ", "");
          var versionDate = DateTime.ParseExact(rawDate, "MMMM dd, yyyy", null);

          versions.Add(new VersionInfo(version, versionDate));
        }
        _packages[name] = versions;
        return versions;

      } catch (Exception e) {
        throw new DependencyNotFoundException(name, e);
      }
    }

    public VersionInfo LatestAsOf(DateTime date, string name) {
      try {
        return GetReleaseHistory(name).OrderByDescending(v => v).
          First(v => date >= v.DatePublished);
      } catch (Exception e) {
        throw new LatestVersionNotFoundException(name, date, e);
      }
    }

    public VersionInfo VersionInfo(string name, string version) {
      return GetReleaseHistory(name).First(v => v.Version == version);
    }

    public VersionInfo Latest(string name, string thatMatches, DateTime asOf) {
      throw new NotImplementedException();
    }
  }
}
