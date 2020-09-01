using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LibGit2Sharp;

namespace Freshli {
  public class GitFileHistory : IFileHistory {
    private readonly IDictionary<DateTime, FileHistory> _historyByDate =
      new Dictionary<DateTime, FileHistory>();

    private string _repositoryPath;
    private string _targetFile;

    public GitFileHistory(string repositoryPath, string targetFile) {
      if (!Directory.Exists(repositoryPath)) {
        var uniqueTempDir = Path.GetFullPath(
          Path.Combine(
            Path.GetTempPath(),
            Guid.NewGuid().ToString()
          )
        );
        Directory.CreateDirectory(uniqueTempDir);
        Repository.Clone(repositoryPath, uniqueTempDir);
        _repositoryPath = uniqueTempDir;
      } else {
        _repositoryPath = repositoryPath;
      }

      _targetFile = targetFile;

      using (var repository = new Repository(repositoryPath))
      {
        var logEntries =
          repository.Commits.
            QueryBy(
              new CommitFilter {
                SortBy = CommitSortStrategies.Topological
              }
            ).Where(c => c.Tree[targetFile] != null);

        foreach (var logEntry in logEntries) {
          var blob = logEntry.Tree[targetFile].Target as Blob;
          var contents = blob.GetContentText();
          var date = logEntry.Committer.When.Date;
          _historyByDate[date] =
            new FileHistory(date, logEntry.Sha, contents);
        }
      }
    }

    public string ContentsAsOf(DateTime date) {
      return _historyByDate[GetKey(date)].Contents;
    }

    public string ShaAsOf(DateTime date) {
      return _historyByDate[GetKey(date)].CommitSha;
    }

    public IList<DateTime> Dates {
      get { return _historyByDate.Keys.OrderBy(d => d).ToList(); }
    }

    private DateTime GetKey(DateTime date)
    {
      return Dates.Last(d => d <= date);
    }

  }

  public class FileHistory {
    public DateTime Date;
    public string CommitSha;
    public string Contents;

    public FileHistory(DateTime date, string commitSha, string contents) {
      Date = date;
      CommitSha = commitSha;
      Contents = contents;
    }
  }
}
