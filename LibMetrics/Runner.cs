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
      var fileHistoryFinder = new FileHistoryFinder(analysisPath);
      if (manifestFinder.Successful)
      {
        var calculator = manifestFinder.Calculator;

        var fileHistory = fileHistoryFinder.FileHistoryOf(
          manifestFinder.LockFileName);

        var analysisDates = new AnalysisDates(fileHistory, asOf);
        foreach (var currentDate in analysisDates)
        {
          var content = fileHistory.ContentsAsOf(currentDate);
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
