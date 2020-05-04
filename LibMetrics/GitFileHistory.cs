using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LibGit2Sharp;

namespace LibMetrics
{
  public class GitFileHistory: IFileHistory
  {
    private readonly List<DateTime> _dates = new List<DateTime>();
    private string _repositoryPath;
    private string _targetFile;

    public GitFileHistory(string repositoryPath, string targetFile)
    {
      if (!Directory.Exists(repositoryPath))
      {
        var uniqueTempDir = Path.GetFullPath(
          Path.Combine(Path.GetTempPath(), 
          Guid.NewGuid().ToString())
        );
        Directory.CreateDirectory(uniqueTempDir);
        Repository.Clone(repositoryPath, uniqueTempDir);
        _repositoryPath = uniqueTempDir;
      } 
      else
      {
        _repositoryPath = repositoryPath;
      }

      _targetFile = targetFile;

      using (var repository = new Repository(repositoryPath))
      {
        var entries = repository.Commits.QueryBy(targetFile).
          OrderBy(entry => entry.Commit.Author.When);

        _dates.AddRange(
          entries.Select(entry => entry.Commit.Author.When.Date).ToList());
      }
    }

    public string ContentsAsOf(DateTime date)
    {
      string result = null;
      using (var repository = new Repository(_repositoryPath))
      {
        var entries = repository.Commits.QueryBy(_targetFile).
          OrderBy(entry => entry.Commit.Author.When);

        var entry = entries.Last(entry => entry.Commit.Author.When.Date <= date);
        var blob = entry.Commit.Tree[entry.Path].Target as Blob;
        result = blob.GetContentText();
      }

      return result;
    }



    public IList<DateTime> Dates => _dates;
  }
}
