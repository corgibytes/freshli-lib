using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Corgibytes.Freshli.Lib.Exceptions;
using NuGet.Common;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;

namespace Corgibytes.Freshli.Lib.Languages.CSharp
{
    public class NuGetRepository : IPackageRepository
    {

        private IDictionary<string, IEnumerable<FreshliNuGetVersionInfo>> _packages
          = new Dictionary<string, IEnumerable<FreshliNuGetVersionInfo>>();

        private IEnumerable<IVersionInfo> GetReleaseHistory(
          string name,
          bool includePreReleaseVersions
        )
        {
            if (_packages.ContainsKey(name))
            {
                return _packages[name];
            }

            var versions = GetMetadata(name);
            _packages[name] = versions
              .OrderByDescending(nv => nv.Published)
              .Select(v => new FreshliNuGetVersionInfo(
                v.Identity.Version,
                v.Published.Value.UtcDateTime
              ));

            return _packages[name];
        }

        private IEnumerable<IPackageSearchMetadata> GetMetadata(string name)
        {
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
          DateTimeOffset asOf,
          bool includePreReleases)
        {
            try
            {
                return GetReleaseHistory(name, includePreReleases)
                .First(v => asOf >= v.DatePublished);
            }
            catch (Exception e)
            {
                throw new LatestVersionNotFoundException(name, asOf, e);
            }
        }

        public IVersionInfo VersionInfo(string name, string version)
        {
            try
            {
                return GetReleaseHistory(name, includePreReleaseVersions: true)
                .First(v => v.Version == version);
            }
            catch (Exception e)
            {
                throw new VersionNotFoundException(name, version, e);
            }
        }

        public List<IVersionInfo> VersionsBetween(
          string name,
          DateTimeOffset asOf,
          IVersionInfo earlierVersion,
          IVersionInfo laterVersion,
          bool includePreReleases)
        {
            return GetReleaseHistory(name, includePreReleases)
              .Where(v => asOf >= v.DatePublished)
              .Where(predicate: v => v.CompareTo(earlierVersion) == 1)
              .Where(predicate: v => v.CompareTo(laterVersion) == -1)
              .ToList();
        }

        public IVersionInfo Latest(
          string name,
          DateTimeOffset asOf,
          string thatMatches
        )
        {
            throw new NotImplementedException();
        }
    }
}
