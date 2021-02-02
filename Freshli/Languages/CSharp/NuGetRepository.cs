using System;
using System.Collections.Generic;
using System.Linq;
using Freshli.Exceptions;
using HtmlAgilityPack;

namespace Freshli.Languages.CSharp {
  public class NuGetRepository : IPackageRepository {

    private IDictionary<string, IList<IVersionInfo>> _packages =
      new Dictionary<string, IList<IVersionInfo>>();

    private IEnumerable<IVersionInfo> GetReleaseHistory(
      string name,
      bool includePreReleaseVersions)
    {
      return new List<IVersionInfo>();
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
