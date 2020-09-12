using System;
using System.Collections.Generic;
using NLog;

namespace Freshli {
  public class Runner {
    private static readonly Logger logger = LogManager.GetCurrentClassLogger();

    public ManifestFinder ManifestFinder { get; private set; }

    public IList<MetricsResult> Run(string analysisPath, DateTime asOf) {
      logger.Info($"Run({analysisPath}, {asOf:d})");

      var metricsResults = new List<MetricsResult>();

      var fileHistoryFinder = new FileHistoryFinder(analysisPath);
      ManifestFinder = new ManifestFinder(
        analysisPath,
        fileHistoryFinder.Finder
      );

      if (ManifestFinder.Successful) {
        logger.Trace(
          "{analysisPath}: LockFileName: {LockFileName}",
          analysisPath,
          ManifestFinder.LockFileName
        );
        var calculator = ManifestFinder.Calculator;

        var fileHistory = fileHistoryFinder.FileHistoryOf(
          ManifestFinder.LockFileName
        );

        var analysisDates = new AnalysisDates(fileHistory, asOf);
        foreach (var currentDate in analysisDates) {
          var content = fileHistory.ContentsAsOf(currentDate);
          calculator.Manifest.Parse(content);

          var sha = fileHistory.ShaAsOf(currentDate);

          LibYearResult libYear = calculator.ComputeAsOf(currentDate);
          logger.Trace(
            "Adding MetricResult: {manifestFile}, " +
            "currentDate = {currentDate:d}, " +
            "sha = {sha}, " +
            "libYear = {ComputeAsOf}",
            ManifestFinder.LockFileName,
            currentDate,
            sha,
            libYear.Total
          );
          metricsResults.Add(new MetricsResult(currentDate, sha, libYear));
        }
      } else {
        logger.Warn("Unable to find a manifest file");
      }

      return metricsResults;
    }

    public IList<MetricsResult> Run(string analysisPath) {
      return Run(analysisPath, asOf: DateTime.Today);
    }
  }
}
