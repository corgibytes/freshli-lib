using System;
using System.Collections.Generic;
using System.Linq;
using Freshli.Util;
using NLog;

namespace Freshli {
  public class Runner {

    private const string ResultsPath = "results";

    private static readonly Logger logger = LogManager.GetCurrentClassLogger();

    public ManifestFinder ManifestFinder { get; private set; }

    public Runner() {
      ManifestFinder.RegisterAll();
      FileHistoryFinder.Register<GitFileHistoryFinder>();
    }

    public IList<MetricsResult> Run(string analysisPath, DateTimeOffset asOf) {
      logger.Info($"Run({analysisPath}, {asOf:O})");

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
      DateTimeOffset asOf,
      List<MetricsResult> metricsResults,
      FileHistoryFinder fileHistoryFinder
    ) {
      logger.Trace(
                "{analysisPath}: LockFileName: {LockFileName}",
                analysisPath,
                ManifestFinder.ManifestFiles[0]
              );
      var calculator = ManifestFinder.Calculator;
      var fileHistory = fileHistoryFinder.FileHistoryOf(
        ManifestFinder.ManifestFiles[0]
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
      DateTimeOffset currentDate
    ) {
      var content = fileHistory.ContentsAsOf(currentDate);
      calculator.Manifest.Parse(content);

      var sha = fileHistory.ShaAsOf(currentDate);

      LibYearResult libYear = calculator.ComputeAsOf(currentDate);
      logger.Trace(
        "Adding MetricResult: {manifestFile}, " +
        "currentDate = {currentDate:O}, " +
        "sha = {sha}, " +
        "libYear = {ComputeAsOf}",
        ManifestFinder.ManifestFiles[0],
        currentDate,
        sha,
        libYear.Total
      );
      metricsResults.Add(new MetricsResult(currentDate, sha, libYear));
    }

    public IList<MetricsResult> Run(string analysisPath) {
      var asOf = DateTimeOffset.Now.ToEndOfDay();
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
