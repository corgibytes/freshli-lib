using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using RestSharp;

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
