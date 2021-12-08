using System;
using System.Collections.Generic;
using System.Text.Json;

namespace Corgibytes.Freshli.Lib.Languages.Php
{
    public class MulticastComposerRepository : IPackageRepository
    {
        private readonly string _projectRootPath;

        private IFileHistory _fileHistory;

        private IPackageRepository _packagistRepository;
        private IDictionary<DateTimeOffset, IDictionary<string, IPackageRepository>> _otherRepositoriesByDate;

        private DateTimeOffset _today = DateTimeOffset.Now;

        public MulticastComposerRepository(string projectRootPath, IFileHistory fileHistory)
        {
            _fileHistory = fileHistory;
            _projectRootPath = projectRootPath;
            _packagistRepository = new ComposerRepository("https://packagist.org");
            _otherRepositoriesByDate = new Dictionary<DateTimeOffset, IDictionary<string, IPackageRepository>>();
        }

        private IDictionary<string, IPackageRepository> CachedRepositoriesFor(DateTimeOffset asOf)
        {
            var repositories = _otherRepositoriesByDate[asOf];
            if (repositories == null)
            {
                repositories = new Dictionary<string, IPackageRepository>();
                _otherRepositoriesByDate[asOf] = repositories;
            }

            return repositories;
        }

        private IPackageRepository CreateRepository(DateTimeOffset asOf, string url)
        {
            var cachedRepositories = CachedRepositoriesFor(asOf);
            var repository = cachedRepositories[url];
            if (repository == null)
            {
                repository = new ComposerRepository(url);
                cachedRepositories[url] = repository;
            }

            return repository;
        }


        private IEnumerable<IPackageRepository> ComposerRepositories(DateTimeOffset asOf)
        {
            yield return _packagistRepository;

            using var composerJson = JsonDocument.Parse(_fileHistory.ContentsAsOf(asOf));
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
            foreach (var repository in ComposerRepositories(_today))
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
