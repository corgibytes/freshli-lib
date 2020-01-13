using System;
using System.Collections.Generic;
using System.Linq;
using LibGit2Sharp;

namespace LibMetrics
{
  public class GitFileHistory: IFileHistory
  {
    private readonly List<DateTime> _dates = new List<DateTime>();

    public GitFileHistory(string repositoryPath, string targetFile)
    {
      using (var repository = new Repository(repositoryPath))
      {
        var entries = repository.Commits.QueryBy(targetFile).
          OrderBy(entry => entry.Commit.Author.When);

        _dates.AddRange(
          entries.Select(entry => entry.Commit.Author.When.Date).ToList());
      }
    }

    public IList<DateTime> Dates => _dates;
  }
}
