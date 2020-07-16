using System;
using System.Collections.Generic;
using NLog;

namespace Freshli {
  public class Runner {
    private static readonly Logger logger = LogManager.GetCurrentClassLogger();

    public IList<MetricsResult> Run(string analysisPath, DateTime asOf) {
      logger.Info($"Run({analysisPath}, {asOf})");

      var metricsResults = new List<MetricsResult>();

      var fileHistoryFinder = new FileHistoryFinder(analysisPath);
      var manifestFinder = new ManifestFinder(
        analysisPath,
        fileHistoryFinder.Finder
      );
      logger.Trace(
        "{analysisPath}: LockFileName: {LockFileName}",
        analysisPath,
        manifestFinder.LockFileName
      );

      if (manifestFinder.Successful) {
        var calculator = manifestFinder.Calculator;

        var fileHistory = fileHistoryFinder.FileHistoryOf(
          manifestFinder.LockFileName
        );

        var analysisDates = new AnalysisDates(fileHistory, asOf);
        foreach (var currentDate in analysisDates) {
          var content = fileHistory.ContentsAsOf(currentDate);
          calculator.Manifest.Parse(content);

          LibYearResult libYear = calculator.ComputeAsOf(currentDate);
          logger.Trace(
            "Adding MetricResult: " +
            "currentDate = {currentDate}, " +
            "libYear = {ComputeAsOf}",
            manifestFinder.LockFileName,
            currentDate,
            libYear.Total
          );
          metricsResults.Add(
            new MetricsResult(
              currentDate,
              libYear
            )
          );
        }
      }

      return metricsResults;
    }

    public IList<MetricsResult> Run(string analysisPath) {
      return Run(analysisPath, asOf: DateTime.Today);
    }
  }
}
