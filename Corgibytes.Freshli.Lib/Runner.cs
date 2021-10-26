using System;
using System.Collections.Generic;
using System.Linq;
using Corgibytes.Freshli.Lib.Util;
using NLog;

namespace Corgibytes.Freshli.Lib
{
    public class Runner : IRunner
    {
        private const string ResultsPath = "results";

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public ManifestService ManifestService { get; private set; }

        public Runner()
        {
            ManifestFinderRegistry.RegisterAll();
            FileHistoryFinder.Register<GitFileHistoryFinder>();
        }

        public IList<ScanResult> Run(string analysisPath, DateTime asOf)
        {
            logger.Info($"Run({analysisPath}, {asOf:d})");

            IList<ScanResult> scanResults = new List<ScanResult>();
            var fileHistoryFinder = new FileHistoryFinder(analysisPath);
            ManifestService = new ManifestService(
                analysisPath,
                fileHistoryFinder.Finder
            );

            if (!ManifestService.Successful)
            {
                logger.Warn("Unable to find a manifest file");
            }
            else
            {
                scanResults = ProcessManifestFiles(
                    analysisPath,
                    asOf,
                    fileHistoryFinder
                );
            }

            DotNetEnv.Env.Load();
            if (DotNetEnv.Env.GetBool("SAVE_RESULTS_TO_FILE"))
            {
                WriteResultsToFile(scanResults);
            }

            return scanResults;
        }

        public IList<ScanResult> Run(string analysisPath)
        {
            var asOf = DateTime.Today.ToEndOfDay();
            return Run(analysisPath, asOf: asOf);
        }

        private IList<ScanResult> ProcessManifestFiles(string analysisPath, DateTime asOf, FileHistoryFinder fileHistoryFinder)
        {
            var scanResults = new List<ScanResult>();
            foreach (var mf in ManifestService.ManifestFiles)
            {
                logger.Trace(
                    "{analysisPath}: LockFileName: {LockFileName}",
                    analysisPath,
                    ManifestService.ManifestFiles[0]
                );

                var calculator = ManifestService.Calculator;
                var fileHistory = fileHistoryFinder.FileHistoryOf(mf);
                var analysisDates = new AnalysisDates(fileHistory, asOf);
                var metricsResults = analysisDates.Select(
                    ad => ProcessAnalysisDate(mf, calculator, fileHistory, ad)
                ).ToList();

                scanResults.Add(new ScanResult(mf, metricsResults));
            }

            return scanResults;
        }

        private MetricsResult ProcessAnalysisDate(string manifestFile, LibYearCalculator calculator, IFileHistory fileHistory, DateTime currentDate)
        {
            var content = fileHistory.ContentsAsOf(currentDate);
            calculator.Manifest.Parse(content);

            var sha = fileHistory.ShaAsOf(currentDate);

            LibYearResult libYear = calculator.ComputeAsOf(currentDate);
            logger.Trace(
                "Adding MetricResult: {manifestFile}, " +
                    "currentDate = {currentDate:O}, " +
                    "sha = {sha}, " +
                    "libYear = {ComputeAsOf}",
                ManifestService.ManifestFiles[0],
                currentDate,
                sha,
                libYear.Total
            );

            return new MetricsResult(currentDate, sha, libYear);
        }

        private static void WriteResultsToFile(IList<ScanResult> results)
        {
            if (!System.IO.Directory.Exists(ResultsPath))
            {
                System.IO.Directory.CreateDirectory(ResultsPath);
            }
            var dateTime = DateTime.Now;
            var filePath =
                $"{ResultsPath}/{dateTime:yyyy-MM-dd-hhmmssfffffff}-results.txt";
            using var file = new System.IO.StreamWriter(filePath);
            foreach (var result in results)
            {
                file.WriteLine(result);
            }
        }
    }
}
