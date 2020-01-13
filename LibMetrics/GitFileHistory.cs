using System;
using System.Collections.Generic;
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
      _targetFile = targetFile;
      _repositoryPath = repositoryPath;
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
