using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LibGit2Sharp;

namespace Corgibytes.Freshli.Lib
{
    public class GitFileHistory : IFileHistory
    {
        private readonly IDictionary<DateTimeOffset, FileHistory> _historyByDate =
          new Dictionary<DateTimeOffset, FileHistory>();

        private string _repositoryPath;
        private string _targetFile;

        public GitFileHistory(string repositoryPath, string targetFile)
        {
            if (!Directory.Exists(repositoryPath))
            {
                var uniqueTempDir = Path.GetFullPath(
                  Path.Combine(
                    Path.GetTempPath(),
                    Guid.NewGuid().ToString()
                  )
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
                var logEntries =
                  repository.Commits.
                    QueryBy(
                      new CommitFilter
                      {
                          SortBy = CommitSortStrategies.Topological
                      }
                    ).Where(c => GetTreeEntry(c, targetFile) != null);

                foreach (var logEntry in logEntries)
                {
                    var blob = GetTreeEntry(logEntry, targetFile).Target as Blob;
                    var contents = blob.GetContentText();
                    var date = logEntry.Committer.When;
                    _historyByDate[date] =
                      new FileHistory(date, logEntry.Sha, contents);
                }
            }
        }

        public string ContentsAsOf(DateTimeOffset date)
        {
            return _historyByDate[GetKey(date)].Contents;
        }

        public string ShaAsOf(DateTimeOffset date)
        {
            return _historyByDate[GetKey(date)].CommitSha;
        }

        public IList<DateTimeOffset> Dates
        {
            get { return _historyByDate.Keys.OrderBy(d => d).ToList(); }
        }

        private DateTimeOffset GetKey(DateTimeOffset date)
        {
            return Dates.Last(d => d <= date);
        }

        // This will get the TreeEntry regardless of the location in the Git repo
        // This will need to recursively go through each directory and see if
        // the file exists
        private TreeEntry GetTreeEntry(Commit commit, string targetFileName)
        {
            return commit.Tree[targetFileName];
        }
    }

    public class FileHistory
    {
        public DateTimeOffset Date;
        public string CommitSha;
        public string Contents;

        public FileHistory(DateTimeOffset date, string commitSha, string contents)
        {
            Date = date;
            CommitSha = commitSha;
            Contents = contents;
        }
    }
}
