using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LibGit2Sharp;

namespace LibMetrics
{
  public class Runner
  {
    public IList<MetricsResult> Run(string analysisPath, DateTime asOf)
    {
      var metricsResults = new List<MetricsResult>();

      var gemfilePath = Path.Combine(analysisPath, "Gemfile.lock");
      // 1. look for supported dependencies files in the specified path
      if (File.Exists(gemfilePath))
      {
        // 2. look for the past versions of the dependency files
        using (var repository = new Repository(analysisPath))
        {
          var entries = repository.Commits.QueryBy("Gemfile.lock").
            OrderBy(entry => entry.Commit.Author.When);

          // 3. parse the dependency files to compute libyear

          var rubyGems = new RubyGemsRepository();
          var manifest = new BundlerManifest();
          var calculator = new LibYearCalculator(rubyGems, manifest);

          var entryEnumerator = entries.GetEnumerator();
          entryEnumerator.MoveNext();

          var currentEntry = entryEnumerator.Current;
          LogEntry nextEntry = null;
          if (entryEnumerator.MoveNext())
          {
            nextEntry = entryEnumerator.Current;
          }

          var gitFileHistory = new GitFileHistory(analysisPath, "Gemfile.lock");
          var analysisDates = new AnalysisDates(gitFileHistory, asOf);
          foreach (var currentDate in analysisDates)
          {
            var blob = currentEntry.Commit.Tree[currentEntry.Path].Target as Blob;
            manifest.Parse(blob.GetContentText());

            metricsResults.Add(
              new MetricsResult(
                currentDate,
                calculator.ComputeAsOf(currentDate)));

            if (nextEntry != null && currentDate.AddMonths(1) >= nextEntry.Commit.Author.When.Date)
            {
              currentEntry = nextEntry;
              if (entryEnumerator.MoveNext())
              {
                nextEntry = entryEnumerator.Current;
              }
              else
              {
                nextEntry = null;
              }
            }
          }
        }
      }

      return metricsResults;
    }

    public IList<MetricsResult> Run(string analysisPath)
    {
      return Run(analysisPath, asOf: DateTime.Today);
    }
  }
}
