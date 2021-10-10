using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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

        private async Task<IEnumerable<IVersionInfo>> GetReleaseHistory(
            string name,
            bool includePreReleaseVersions
        )
        {
            if (_packages.ContainsKey(name))
            {
                return _packages[name];
            }

            var versions = await GetMetadata(name);
            _packages[name] = versions
                .OrderByDescending(nv => nv.Published)
                .Select(v => new FreshliNuGetVersionInfo(
                    v.Identity.Version,
                    v.Published.Value.UtcDateTime
                )
            );

            return _packages[name];
        }

        private async Task<IEnumerable<IPackageSearchMetadata>> GetMetadata(string name)
        {
            ILogger logger = NullLogger.Instance;
            CancellationToken cancellationToken = CancellationToken.None;

            SourceCacheContext cache = new SourceCacheContext();
            SourceRepository repository = Repository.Factory.GetCoreV3(
                "https://api.nuget.org/v3/index.json"
            );
            PackageMetadataResource resource = await repository.GetResourceAsync<PackageMetadataResource>();

            IEnumerable<IPackageSearchMetadata> packages = await resource.GetMetadataAsync(
                name,
                includePrerelease: true,
                includeUnlisted: false,
                cache,
                logger,
                cancellationToken);

            return packages;
        }

        public IVersionInfo Latest(
            string name,
            DateTime asOf,
            bool includePreReleases)
        {
            try
            {
                return GetReleaseHistory(name, includePreReleases)
                    .Result
                    .First(v => asOf >= v.DatePublished);
            }
            catch (Exception e)
            {
                throw new LatestVersionNotFoundException(name, asOf, e);
            }
        }

        public async Task<IVersionInfo> VersionInfo(string name, string version)
        {
            try
            {
                return (await GetReleaseHistory(name, includePreReleaseVersions: true))
                    .First(v => v.Version == version);
            }
            catch (Exception e)
            {
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
            return GetReleaseHistory(name, includePreReleases)
                .Result
                .Where(v => asOf >= v.DatePublished)
                .Where(predicate: v => v.CompareTo(earlierVersion) == 1)
                .Where(predicate: v => v.CompareTo(laterVersion) == -1)
                .ToList();
        }

        public IVersionInfo Latest(string name, DateTime asOf, string thatMatches)
        {
            throw new NotImplementedException();
        }
    }
}
