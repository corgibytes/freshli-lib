using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace LibMetrics.Languages.Php
{
  public class MulticastComposerRepository: IPackageRepository
  {
    private readonly string _projectRootPath;

    private List<IPackageRepository> _composerRespositories =
      new List<IPackageRepository>();

    public MulticastComposerRepository(string projectRootPath)
    {
      _projectRootPath = projectRootPath;
      _composerRespositories.Add(new ComposerRepository("https://packagist.org"));

      using var composerJson = JsonDocument.Parse(
        File.ReadAllText(
          Path.Combine(projectRootPath, "composer.json")));
      if (composerJson.RootElement.TryGetProperty("repositories", out var repositoryList))
      {
        foreach (var repositoryEntry in repositoryList.EnumerateArray())
        {
          if (repositoryEntry.GetProperty("type").GetString() == "composer")
          {
            _composerRespositories.Add(new ComposerRepository(
              repositoryEntry.GetProperty("url").ToString()));
          }
        }
      }
    }

    public VersionInfo LatestAsOf(DateTime date, string name)
    {
      foreach (var repository in _composerRespositories)
      {
        var result = repository.LatestAsOf(date, name);
        if (result != null)
        {
          return result;
        }
      }

      return null;
    }

    public VersionInfo VersionInfo(string name, string version)
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
  }
}