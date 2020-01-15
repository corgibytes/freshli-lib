using System;
using System.Collections.Generic;

namespace LibMetrics
{
  public class Runner
  {
    public IList<MetricsResult> Run(string analysisPath, DateTime asOf)
    {
      var metricsResults = new List<MetricsResult>();

      var manifestFinder = new ManifestFinder(analysisPath);
      if (manifestFinder.Successful)
      {
        var calculator = manifestFinder.Calculator;

        var gitFileHistory = new GitFileHistory(
          analysisPath,
          manifestFinder.LockFileName);
        var analysisDates = new AnalysisDates(gitFileHistory, asOf);
        foreach (var currentDate in analysisDates)
        {
          var content = gitFileHistory.ContentsAsOf(currentDate);
          calculator.Manifest.Parse(content);

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
