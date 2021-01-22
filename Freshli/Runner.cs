using System;
using System.Collections.Generic;
using System.Linq;
using Freshli.Util;
using System.Linq;
using NLog;

namespace Freshli {
  public class Runner {

    private const string ResultsPath = "results";

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

      if (!ManifestFinder.Successful) {
        logger.Warn("Unable to find a manifest file");
      } else {
        ProcessManifestFile(
          analysisPath,
          asOf,
          metricsResults,
          fileHistoryFinder
        );
      }

      DotNetEnv.Env.Load();
      if (DotNetEnv.Env.GetBool("SAVE_RESULTS_TO_FILE")) {
        WriteResultsToFile(metricsResults);
      }

      return metricsResults;
    }

    private void ProcessManifestFile(
      string analysisPath,
      DateTime asOf,
      List<MetricsResult> metricsResults,
      FileHistoryFinder fileHistoryFinder
    ) {
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
      analysisDates.ToList().ForEach(
        ad => ProcessAnalysisDate(metricsResults, calculator, fileHistory, ad)
      );
    }

    private void ProcessAnalysisDate(
      List<MetricsResult> metricsResults,
      LibYearCalculator calculator,
      IFileHistory fileHistory,
      DateTime currentDate
    ) {
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

    public IList<MetricsResult> Run(string analysisPath) {
      var asOf = DateTime.Today.ToEndOfDay();
      return Run(analysisPath, asOf: asOf);
    }

    private static void WriteResultsToFile(List<MetricsResult> results) {
      if (!System.IO.Directory.Exists(ResultsPath)) {
        System.IO.Directory.CreateDirectory(ResultsPath);
      }
      var dateTime = DateTime.Now;
      var filePath =
        $"{ResultsPath}/{dateTime:yyyy-MM-dd-hhmmssfffffff}-results.txt";
      using var file = new System.IO.StreamWriter(filePath);
      foreach (var result in results) {
        file.WriteLine(result);
      }
    }

  }
}
