using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using LibGit2Sharp;

namespace LibMetrics
{
  public class GitFileHistoryFinder: IFileHistoryFinder
  {
    private Dictionary<string, string> _cloneLocations = new Dictionary<string, string>();
    private string NormalizeLocation(string projectRootPath)
    {
      if (Repository.IsValid(projectRootPath))
      {
        return projectRootPath;
      }

      if (_cloneLocations.ContainsKey(projectRootPath))
      {
        return _cloneLocations[projectRootPath];
      }

      if (IsCloneable(projectRootPath))
      {
        var cloneLocation = GenerateTempCloneLocation();
        Repository.Clone(projectRootPath, cloneLocation);
        _cloneLocations[projectRootPath] = cloneLocation;
        return cloneLocation;
      }

      return projectRootPath;
    }

    public bool DoesPathContainHistorySource(string projectRootPath)
    {
      bool result = Repository.IsValid(projectRootPath);
      if (!result)
      {
        result = IsCloneable(projectRootPath);
      }

      return result;
    }

    private string GenerateTempCloneLocation()
    {
      return Path.Combine(
        Path.GetTempPath(),
        Guid.NewGuid().ToString());
    }

    private bool IsCloneable(string url)
    {
      var result = true;
      var options = new CloneOptions {Checkout = false};

      string tempFolder = GenerateTempCloneLocation();

      try
      {
        Repository.Clone(url, tempFolder, options);
      }
      catch (NotFoundException)
      {
        result = false;
      }

      if (Directory.Exists(tempFolder))
      {
        Directory.Delete(tempFolder, recursive: true);
      }

      return result;
    }

    public IFileHistory FileHistoryOf(string projectRootPath, string targetFile)
    {
      return new GitFileHistory(NormalizeLocation(projectRootPath), targetFile);
    }

    public bool Exists(string projectRootPath, string filePath)
    {
      string clonedProjectRoot = NormalizeLocation(projectRootPath);
      return File.Exists(Path.Combine(clonedProjectRoot, filePath));
    }

    public string ReadAllText(string projectRootPath, string filePath)
    {
      string clonedProjectRoot = NormalizeLocation(projectRootPath);
      return File.ReadAllText(Path.Combine(clonedProjectRoot, filePath));
    }
  }
}
