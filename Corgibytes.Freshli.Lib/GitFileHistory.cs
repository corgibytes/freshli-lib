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

        /// <summary>
        /// The contents for the file for the given date.
        /// </summary>
        /// <param name="date">The date to find the contest at.</param>
        /// <returns>Returns the contents for the file for the given date. If nothing exists
        /// for the given date then an empty string is returned.</returns>
        public string ContentsAsOf(DateTimeOffset date)
        {
            try
            {
                return _historyByDate[GetKey(date)].Contents;
            }
            catch (InvalidOperationException)
            {
                return "";
            }
        }

        /// <summary>
        /// The Git commit SHA for the file at the given date.
        /// </summary>
        /// <param name="date">The date to find the SHA at.</param>
        /// <returns>Returns the sha for the file for the given date. If nothing exists
        /// for the given date then an empty string is returned.</returns>
        public string ShaAsOf(DateTimeOffset date)
        {
            try
            {
                return _historyByDate[GetKey(date)].CommitSha;
            }
            catch (InvalidOperationException)
            {
                return "";
            }
        }

        public IList<DateTimeOffset> Dates
        {
            get { return _historyByDate.Keys.OrderBy(d => d).ToList(); }
        }

        private DateTimeOffset GetKey(DateTimeOffset date)
        {
            // This will fail if the date passed in is greater than
            // the last date in the list.  Should find a better way to
            // deal with dates that aren't found in this class.
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
