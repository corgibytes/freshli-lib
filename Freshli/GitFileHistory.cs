using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LibGit2Sharp;

namespace Freshli
{
  public class GitFileHistory: IFileHistory
  {
    private readonly IDictionary<DateTime, string> _contentsByDate = new Dictionary<DateTime, string>();
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
        var entries = repository.Commits.QueryBy(targetFile,
          new CommitFilter {SortBy = CommitSortStrategies.Topological});

        foreach (var entry in entries)
        {
          var blob = entry.Commit.Tree[entry.Path].Target as Blob;
          var contents = blob.GetContentText();
          _contentsByDate[entry.Commit.Author.When.Date] = contents;
        }
      }
    }

    public string ContentsAsOf(DateTime date)
    {
      var key = Dates.Last(d => d <= date);
      return _contentsByDate[key];
    }

    public IList<DateTime> Dates
    {
      get
      {
        return _contentsByDate.Keys.OrderBy(d => d).ToList();
      }
    }
  }
}
