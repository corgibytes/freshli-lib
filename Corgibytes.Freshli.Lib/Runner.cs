using System;
using System.Collections.Generic;
using System.Linq;
using Corgibytes.Freshli.Lib.Util;
using NLog;

namespace Corgibytes.Freshli.Lib {
  public class Runner {

    private const string ResultsPath = "results";

    private static readonly Logger logger = LogManager.GetCurrentClassLogger();

    public ManifestFinder ManifestFinder { get; private set; }

    public Runner() {
      ManifestFinder.RegisterAll();
      FileHistoryFinder.Register<GitFileHistoryFinder>();
    }

    public IList<ScanResult> Run(string analysisPath, DateTime asOf) {
      logger.Info($"Run({analysisPath}, {asOf:d})");

      var fileHistoryFinder = new FileHistoryFinder(analysisPath);
      ManifestFinder = new ManifestFinder(
        analysisPath,
        fileHistoryFinder.Finder
      );

      if (!ManifestFinder.Successful) {
        logger.Warn("Unable to find a manifest file");
        // do we need to bother processing the rest here?
        return null;
      }

      var scanResults = ProcessManifestFiles(
        analysisPath,
        asOf,
        fileHistoryFinder
      );

      // DotNetEnv.Env.Load();
      // if (DotNetEnv.Env.GetBool("SAVE_RESULTS_TO_FILE")) {
      //   WriteResultsToFile(scanResults);
      // }

      return scanResults;
    }

    private IList<ScanResult> ProcessManifestFiles(
      string analysisPath,
      DateTime asOf,
      FileHistoryFinder fileHistoryFinder
    ) {
      var calculator = ManifestFinder.Calculator;
      var scanResults = new List<ScanResult>();
      foreach (var mf in ManifestFinder.ManifestFiles) {
        logger.Trace(
          "{analysisPath}: LockFileName: {LockFileName}",
          analysisPath,
          mf
        );

        var fileHistory = fileHistoryFinder.FileHistoryOf(mf);
        var analysisDates = new AnalysisDates(fileHistory, asOf);
        var metricsResults = analysisDates.Select(ad => ProcessAnalysisDate(calculator, fileHistory, ad)).ToList();

        scanResults.Add(
          new ScanResult(mf, metricsResults)
        );
      }

      return scanResults;
    }

    private MetricsResult ProcessAnalysisDate(
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
        ManifestFinder.ManifestFiles[0],
        currentDate,
        sha,
        libYear.Total
      );
      return new MetricsResult(currentDate, sha, libYear);
    }

    public IList<ScanResult> Run(string analysisPath) {
      var asOf = DateTime.Today.ToEndOfDay();
      return Run(analysisPath, asOf: asOf);
    }

    private static void WriteResultsToFile(List<ScanResult> results) {
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
