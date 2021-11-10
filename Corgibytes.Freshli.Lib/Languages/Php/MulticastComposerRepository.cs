using System;
using System.Collections.Generic;
using System.Text.Json;

namespace Corgibytes.Freshli.Lib.Languages.Php
{
    public class MulticastComposerRepository : IPackageRepository
    {
        private readonly string _projectRootPath;

        private List<IPackageRepository> _composerRespositories =
          new List<IPackageRepository>();

        private IFileHistoryFinder _fileHistoryFinder;

        public MulticastComposerRepository(
          string projectRootPath,
          IFileHistoryFinder fileHistoryFinder
        )
        {
            _fileHistoryFinder = fileHistoryFinder;
            _projectRootPath = projectRootPath;
            _composerRespositories.Add(
              new ComposerRepository("https://packagist.org")
            );

            using var composerJson = JsonDocument.Parse(
              _fileHistoryFinder.ReadAllText(projectRootPath, "composer.json")
            );
            if (composerJson.RootElement.TryGetProperty(
              "repositories",
              out var repositoryList
            ))
            {
                foreach (var repositoryEntry in repositoryList.EnumerateArray())
                {
                    if (repositoryEntry.GetProperty("type").GetString() == "composer")
                    {
                        _composerRespositories.Add(
                          new ComposerRepository(
                            repositoryEntry.GetProperty("url").ToString()
                          )
                        );
                    }
                }
            }
        }

        public IVersionInfo Latest(
          string name,
          DateTimeOffset asOf,
          bool includePreReleases)
        {
            foreach (var repository in _composerRespositories)
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
            foreach (var repository in _composerRespositories)
            {
                var result = repository.VersionInfo(name, version);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }

        public IVersionInfo Latest(
          string name,
          DateTimeOffset asOf,
          string thatMatches
        )
        {
            throw new NotImplementedException();
        }

        public List<IVersionInfo> VersionsBetween(
          string name,
          DateTimeOffset asOf,
          IVersionInfo earlierVersion,
          IVersionInfo laterVersion,
          bool includePreReleases
        )
        {
            //TODO: Implement method
            throw new NotImplementedException();
        }
    }
}
