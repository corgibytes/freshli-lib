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

            // TODO: inject this dependency
            ManifestService = new ManifestService();
        }

        public IList<ScanResult> Run(string analysisPath, DateTime asOf)
        {
            logger.Info($"Run({analysisPath}, {asOf:d})");

            IList<ScanResult> scanResults = new List<ScanResult>();

            // TODO: inject this dependencies
            // TODO: rename to FileHistoryService
            var fileHistoryFinder = new FileHistoryFinder(analysisPath);

            var manifestFinders = ManifestService.SelectFindersFor(analysisPath, fileHistoryFinder.Finder);
            IEnumerable<AbstractManifestFinder> abstractManifestFinders = manifestFinders as AbstractManifestFinder[] ?? manifestFinders.ToArray();
            if (!abstractManifestFinders.Any(finder => finder.GetManifestFilenames(analysisPath).Length > 0))
            {
                logger.Warn("Unable to find a manifest file");
            }
            else
            {
                scanResults = ProcessManifestFiles(
                    analysisPath,
                    asOf,
                    abstractManifestFinders,
                    fileHistoryFinder
                ).ToList();
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

        private IEnumerable<ScanResult> ProcessManifestFiles(string analysisPath, DateTimeOffset asOf, IEnumerable<AbstractManifestFinder> manifestFinders, FileHistoryFinder fileHistoryFinder)
        {
            foreach (var manifestFinder in manifestFinders)
            {
                foreach (var manifestFile in manifestFinder.GetManifestFilenames(analysisPath))
                {
                    logger.Trace(
                        "{analysisPath}: LockFileName: {LockFileName}",
                        analysisPath,
                        manifestFile
                    );

                    var calculator = new LibYearCalculator(
                        manifestFinder.RepositoryFor(manifestFile),
                        manifestFinder.ManifestFor(manifestFile)
                    );

                    var fileHistory = fileHistoryFinder.FileHistoryOf(manifestFile);
                    var analysisDates = new AnalysisDates(fileHistory, asOf.DateTime);
                    var metricsResults = analysisDates.Select(
                        ad => ProcessAnalysisDate(manifestFile, calculator, fileHistory, ad)
                    ).ToList();

                    yield return new ScanResult(manifestFile, metricsResults);

                    // TODO: Remove this break to enable multi-manifest file support, I _think_ ^_^
                    yield break;
                }
            }
        }

        private MetricsResult ProcessAnalysisDate(string manifestFile, LibYearCalculator calculator, IFileHistory fileHistory, DateTime currentDate)
        {
            var content = fileHistory.ContentsAsOf(currentDate);
            // TODO: The manifest should be retreived from the ManifestFinder, not the calculator
            calculator.Manifest.Parse(content);

            var sha = fileHistory.ShaAsOf(currentDate);

            LibYearResult libYear = calculator.ComputeAsOf(currentDate);
            logger.Trace(
                "Adding MetricResult: {manifestFile}, " +
                    "currentDate = {currentDate:O}, " +
                    "sha = {sha}, " +
                    "libYear = {ComputeAsOf}",
                manifestFile,
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
