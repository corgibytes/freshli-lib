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

        public IManifestService ManifestService { get; init; }

        public FileHistoryService FileHistoryService { get; init; }

        public Runner(IManifestService manifestService, FileHistoryService fileHistoryService)
        {
            ManifestService = manifestService;
            FileHistoryService = FileHistoryService;
        }

        // TODO: Move this method to `ManifestService`
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

            var fileHistoryFinder = FileHistoryService.SelectFinderFor(analysisPath);

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

                    var fileHistory = fileHistoryFinder.FileHistoryOf(analysisPath, manifestFile);
                    var analysisDates = new AnalysisDates(fileHistory, asOf.DateTime);
                    var metricsResults = analysisDates.Select(
                        ad => ProcessAnalysisDate(manifestFile, manifestFinder, fileHistory, ad)
                    ).ToList();

                    yield return new ScanResult(manifestFile, metricsResults);

                    // TODO: Remove this break to enable multi-manifest file support, I _think_ ^_^
                    yield break;
                }
            }
        }

        private MetricsResult ProcessAnalysisDate(string manifestFile, IManifestFinder manifestFinder, IFileHistory fileHistory, DateTimeOffset currentDate)
        {
            using var contentStream = fileHistory.ContentStreamAsOf(currentDate);

            var manifestParser = manifestFinder.ManifestParser();
            var parsedManifestContents = manifestParser.Parse(contentStream);
            var calculator = new LibYearCalculator(
                // TODO: the repository that's used should be based on the `manifestFile` not on the repository root
                manifestFinder.RepositoryFor(null),
                // TODO: manifest should be provided by a manifest parser
                parsedManifestContents,
                manifestParser.UsesExactMatches
            );


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
