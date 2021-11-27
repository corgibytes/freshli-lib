using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Corgibytes.Freshli.Lib.Util;
using Microsoft.Extensions.Logging;
using NLog;

namespace Corgibytes.Freshli.Lib
{
    public class Runner : IRunner
    {
        private const string ResultsPath = "results";

        private ILoggerFactory _loggerFactory;

        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        public IManifestService ManifestService { get; init; }

        public IFileHistoryService FileHistoryService { get; init; }

        public Runner(IManifestService manifestService, IFileHistoryService fileHistoryService, ILoggerFactory loggerFactory)
        {
            ManifestService = manifestService;
            FileHistoryService = fileHistoryService;

            _loggerFactory = loggerFactory;
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

        public IEnumerable<ScanResult> Run(string analysisPath, DateTimeOffset asOf)
        {
            logger.Info($"Run({analysisPath}, {asOf:O})");

            var streamWriter = TextWriter.Null;
            // TODO: Remove dependency on `DotNetEnv` for this feature
            DotNetEnv.Env.Load();
            if (DotNetEnv.Env.GetBool("SAVE_RESULTS_TO_FILE"))
            {
                streamWriter = CreateResultsToFileWriter();
            }

            var fileHistoryFinder = FileHistoryService.SelectFinderFor(analysisPath);

            var manifestFinders = ManifestService.SelectFindersFor(analysisPath, fileHistoryFinder);
            if (!(ContainsManifestFile(manifestFinders, analysisPath)))
            {
                logger.Warn("Unable to find a manifest file");
            }
            else
            {
                // TODO: Remove the call to `Take(1)` to support results from multiple manifest files
                foreach(var result in ProcessManifestFiles(analysisPath, asOf, manifestFinders, fileHistoryFinder).Take(1))
                {
                    yield return result;
                    streamWriter.WriteLine(result.ToString());
                }
            }
            streamWriter.Close();
            // TODO: switch to a `using` block
            streamWriter.Dispose();
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
                        ad => ProcessAnalysisDate(analysisPath, manifestFile, manifestFinder, fileHistory, ad)
                    ).ToList();

                    yield return new ScanResult(manifestFile, metricsResults);

                    // TODO: Remove this break to enable multi-manifest file support, I _think_ ^_^
                    yield break;
                }
            }
        }

        private MetricsResult ProcessAnalysisDate(string analysisPath, string manifestFile, IManifestFinder manifestFinder, IFileHistory fileHistory, DateTimeOffset currentDate)
        {
            using var contentStream = fileHistory.ContentStreamAsOf(currentDate);

            var manifestParser = manifestFinder.ManifestParser();
            var parsedManifestContents = manifestParser.Parse(contentStream);
            var calculator = new LibYearCalculator(
                // TODO: the repository that's used should be based on the `manifestFile` not on the repository root
                manifestFinder.RepositoryFor(analysisPath),
                // TODO: manifest should be provided by a manifest parser
                parsedManifestContents,
                manifestParser.UsesExactMatches,
                _loggerFactory.CreateLogger<LibYearCalculator>()
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

        public IEnumerable<ScanResult> Run(string analysisPath)
        {
            var asOf = DateTimeOffset.UtcNow.ToEndOfDay();
            var results = Run(analysisPath, asOf: asOf);
            foreach (var result in results)
            {
                yield return result;
            }
        }

        private TextWriter CreateResultsToFileWriter()
        {
            if (!System.IO.Directory.Exists(ResultsPath))
            {
                System.IO.Directory.CreateDirectory(ResultsPath);
            }
            var dateTime = DateTimeOffset.UtcNow;
            var filePath =
                $"{ResultsPath}/{dateTime:yyyy-MM-dd-hhmmssfffffff}-results.txt";
            return new System.IO.StreamWriter(filePath);
        }
    }
}
