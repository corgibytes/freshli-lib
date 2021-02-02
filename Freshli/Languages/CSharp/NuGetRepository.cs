using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Freshli.Exceptions;
using NuGet.Common;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;

namespace Freshli.Languages.CSharp {
  public class NuGetRepository : IPackageRepository {

    private IDictionary<string, IList<IVersionInfo>> _packages =
      new Dictionary<string, IList<IVersionInfo>>();

    private IEnumerable<IVersionInfo> GetReleaseHistory(
      string name,
      bool includePreReleaseVersions) {
      var versions = GetMetadata(name);

      return versions.Select(v => new FreshliNuGetVersionInfo(v));
    }

    private IEnumerable<IPackageSearchMetadata> GetMetadata(string name) {
      ILogger logger = NullLogger.Instance;
      CancellationToken cancellationToken = CancellationToken.None;

      SourceCacheContext cache = new SourceCacheContext();
      SourceRepository repository = Repository.Factory.GetCoreV3(
        "https://api.nuget.org/v3/index.json"
      );
      PackageMetadataResource resource =
        repository.GetResourceAsync<PackageMetadataResource>().Result;

      IEnumerable<IPackageSearchMetadata> packages = resource.GetMetadataAsync(
          name,
          includePrerelease: true,
          includeUnlisted: false,
          cache,
          logger,
          cancellationToken).Result;

      return packages;
    }

    public IVersionInfo Latest(
      string name,
      DateTime asOf,
      bool includePreReleases)
    {
      try {
        return GetReleaseHistory(name, includePreReleases).
          OrderByDescending(v => v.DatePublished).
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

    public IVersionInfo Latest(string name, DateTime asOf, string thatMatches) {
      throw new NotImplementedException();
    }
  }
}
