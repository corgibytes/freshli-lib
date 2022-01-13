using System;
using System.Collections.Generic;
using System.Text.Json;

namespace Corgibytes.Freshli.Lib.Languages.Php
{
    public class MulticastComposerRepository : IPackageRepository
    {
        private IFileHistory _jsonFileHistory;

        private IPackageRepository _packagistRepository;
        private IDictionary<DateTimeOffset, IDictionary<string, IPackageRepository>> _otherRepositoriesByDate;

        private DateTimeOffset _today = DateTimeOffset.Now;

        public MulticastComposerRepository(IFileHistory jsonFileHistory)
        {
            _jsonFileHistory = jsonFileHistory;
            _packagistRepository = new ComposerRepository("https://packagist.org");
            _otherRepositoriesByDate = new Dictionary<DateTimeOffset, IDictionary<string, IPackageRepository>>();
        }

        private IDictionary<string, IPackageRepository> CachedRepositoriesFor(DateTimeOffset asOf)
        {
            if (!_otherRepositoriesByDate.ContainsKey(asOf))
            {
                _otherRepositoriesByDate[asOf] = new Dictionary<string, IPackageRepository>();
            }

            return _otherRepositoriesByDate[asOf];
        }

        private IPackageRepository CreateRepository(DateTimeOffset asOf, string url)
        {
            var cachedRepositories = CachedRepositoriesFor(asOf);
            if (!cachedRepositories.ContainsKey(url))
            {
                cachedRepositories[url] = new ComposerRepository(url);
            }

            return cachedRepositories[url];
        }


        private IEnumerable<IPackageRepository> ComposerRepositories(DateTimeOffset asOf)
        {
            yield return _packagistRepository;

            using var composerJson = JsonDocument.Parse(_jsonFileHistory.ContentsAsOf(asOf));
            if (composerJson.RootElement.TryGetProperty("repositories", out var repositoryList))
            {
                foreach (var repositoryEntry in repositoryList.EnumerateArray())
                {
                    if (repositoryEntry.GetProperty("type").GetString() == "composer")
                    {
                        yield return CreateRepository(asOf, repositoryEntry.GetProperty("url").ToString());
                    }
                }
            }
        }

        public IVersionInfo Latest(string name, DateTimeOffset asOf, bool includePreReleases)
        {
            foreach (var repository in ComposerRepositories(asOf))
            {
                var result = repository.Latest(name, asOf, includePreReleases);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }

        public IVersionInfo VersionInfo(string name, string version)
        {
            foreach (var repository in ComposerRepositories(_today)) // TODO: instead of `_today` this should be the analysis date
            {
                var result = repository.VersionInfo(name, version);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }

        public IVersionInfo Latest(string name, DateTimeOffset asOf, string thatMatches)
        {
            // TODO: Implement this method
            throw new NotImplementedException();
        }

        public List<IVersionInfo> VersionsBetween(string name, DateTimeOffset asOf, IVersionInfo earlierVersion, IVersionInfo laterVersion, bool includePreReleases)
        {
            //TODO: Implement method
            throw new NotImplementedException();
        }
    }
}
