using System;
using System.Collections.Generic;
using System.Text.Json;

namespace Freshli.Languages.Php {
  public class MulticastComposerRepository : IPackageRepository {
    private readonly string _projectRootPath;

    private List<IPackageRepository> _composerRespositories =
      new List<IPackageRepository>();

    private IFileHistoryFinder _fileHistoryFinder;

    public MulticastComposerRepository(
      string projectRootPath,
      IFileHistoryFinder fileHistoryFinder
    ) {
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
      )) {
        foreach (var repositoryEntry in repositoryList.EnumerateArray()) {
          if (repositoryEntry.GetProperty("type").GetString() == "composer") {
            _composerRespositories.Add(
              new ComposerRepository(
                repositoryEntry.GetProperty("url").ToString()
              )
            );
          }
        }
      }
    }

    public IVersionInfo LatestAsOf(string name, DateTime asOf) {
      foreach (var repository in _composerRespositories) {
        var result = repository.LatestAsOf(name, asOf);
        if (result != null) {
          return result;
        }
      }

      return null;
    }

    public IVersionInfo VersionInfo(string name, string version) {
      foreach (var repository in _composerRespositories) {
        var result = repository.VersionInfo(name, version);
        if (result != null) {
          return result;
        }
      }

      return null;
    }

    public IVersionInfo LatestAsOfThatMatches(
      string name,
      DateTime asOf,
      string thatMatches
    ) {
      throw new NotImplementedException();
    }
  }
}
