using System;
using System.Collections.Generic;
using System.IO;

namespace LibMetrics
{
  public class Runner
  {
    public IList<MetricsResult> Run(string analysisPath, DateTime asOf)
    {
      var metricsResults = new List<MetricsResult>();

      var gemfilePath = Path.Combine(analysisPath, "Gemfile.lock");
      if (File.Exists(gemfilePath))
      {
        var rubyGems = new RubyGemsRepository();
        var manifest = new BundlerManifest();
        var calculator = new LibYearCalculator(rubyGems, manifest);

        var gitFileHistory = new GitFileHistory(analysisPath, "Gemfile.lock");
        var analysisDates = new AnalysisDates(gitFileHistory, asOf);
        foreach (var currentDate in analysisDates)
        {
          var content = gitFileHistory.ContentsAsOf(currentDate);
          manifest.Parse(content);

          metricsResults.Add(
            new MetricsResult(
              currentDate,
              calculator.ComputeAsOf(currentDate)));
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
