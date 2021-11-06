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

        public IFileHistoryFinderRegistry FileHistoryFinderRegistry { get; private set; }

        public Runner()
        {
            // TODO: The manifest finder registry should be injected
            ManifestFinderRegistry.RegisterAll();

            // TODO: The file history registry should be injected
            FileHistoryFinderRegistry = new FileHistoryFinderRegistry();
            FileHistoryFinderRegistry.Register<GitFileHistoryFinder>();
            FileHistoryFinderRegistry.Register<LocalFileHistoryFinder>();

            // TODO: inject this dependency
            ManifestService = new ManifestService();
        }

        private bool ContainsManifestFile(IEnumerable<IManifestFinder> manifestFinders, string analysisPath)
        {
            foreach (var manifestFinder in manifestFinders)
            {
                foreach (string fileName in manifestFinder.GetManifestFilenames(analysisPath))
                {
                    return true;
                }
            }

            return false;
        }

        public IList<ScanResult> Run(string analysisPath, DateTimeOffset asOf)
        {
            logger.Info($"Run({analysisPath}, {asOf:O})");

            IList<ScanResult> scanResults = new List<ScanResult>();

            // TODO: inject this dependencies
            var fileHistoryService = new FileHistoryService(FileHistoryFinderRegistry);
            var fileHistoryFinder = fileHistoryService.SelectFinderFor(analysisPath);

            var manifestFinders = ManifestService.SelectFindersFor(analysisPath, fileHistoryFinder);
            if (!ContainsManifestFile(manifestFinders, analysisPath))
            {
                logger.Warn("Unable to find a manifest file");
            }
            else
            {
                scanResults = ProcessManifestFiles(
                    analysisPath,
                    asOf,
                    manifestFinders,
                    fileHistoryFinder
                // TODO: Remove the call to `Take(1)` to support results from multiple manifest files
                ).Take(1).ToList();
            }

            DotNetEnv.Env.Load();
            if (DotNetEnv.Env.GetBool("SAVE_RESULTS_TO_FILE"))
            {
                WriteResultsToFile(scanResults);
            }

            return scanResults;
        }

        private IEnumerable<ScanResult> ProcessManifestFiles(string analysisPath, DateTimeOffset asOf, IEnumerable<IManifestFinder> manifestFinders, IFileHistoryFinder fileHistoryFinder)
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

                    // TODO: LibYear calculation should not happen from inside the constructor. Switch to using a `Compute` or `Calculate` method instead.
                    var calculator = new LibYearCalculator(
                        // TODO: the repository that's used should be based on the `manifestFile` not on the repository root
                        manifestFinder.RepositoryFor(analysisPath),
                        // TODO: manifest should be provided by a manifest parser
                        manifestFinder.ManifestFor(analysisPath)
                    );

                    var fileHistory = fileHistoryFinder.FileHistoryOf(analysisPath, manifestFile);
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

        private MetricsResult ProcessAnalysisDate(string manifestFile, LibYearCalculator calculator, IFileHistory fileHistory, DateTimeOffset currentDate)
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

        public IList<ScanResult> Run(string analysisPath)
        {
            var asOf = DateTimeOffset.UtcNow.ToEndOfDay();
            return Run(analysisPath, asOf: asOf);
        }

        private static void WriteResultsToFile(IList<ScanResult> results)
        {
            if (!System.IO.Directory.Exists(ResultsPath))
            {
                System.IO.Directory.CreateDirectory(ResultsPath);
            }
            var dateTime = DateTimeOffset.UtcNow;
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
