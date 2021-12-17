using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Corgibytes.Freshli.Lib.Util;
using Microsoft.Extensions.Logging;

namespace Corgibytes.Freshli.Lib
{
    public class Runner : IRunner
    {
        private const string ResultsPath = "results";

        private ILoggerFactory _loggerFactory;

        private readonly ILogger<Runner> _logger;

        public IManifestFinderRegistry ManifestFinderRegistry { get; init; }

        public IFileHistoryFinderRegistry FileHistoryFinderRegistry { get; init; }

        public Runner(IManifestFinderRegistry manifestFinderRegistry, IFileHistoryFinderRegistry fileHistoryFinderRegistry, ILoggerFactory loggerFactory)
        {
            ManifestFinderRegistry = manifestFinderRegistry;
            FileHistoryFinderRegistry = fileHistoryFinderRegistry;

            _loggerFactory = loggerFactory;
            _logger = loggerFactory.CreateLogger<Runner>();
        }

        public IEnumerable<ScanResult> Run(string analysisPath, DateTimeOffset asOf)
        {
            _logger.LogInformation($"Run({analysisPath}, {asOf:O})");

            var streamWriter = TextWriter.Null;
            // TODO: Remove dependency on `DotNetEnv` for this feature
            DotNetEnv.Env.Load();
            if (DotNetEnv.Env.GetBool("SAVE_RESULTS_TO_FILE"))
            {
                streamWriter = CreateResultsToFileWriter();
            }

            var fileHistorySource = FileHistoryFinderRegistry.Finders.Select(f => f.HistorySourceFor(analysisPath)).First(f => f.ContainsFileHistory);

            // TODO: Remove the call to `Take(1)` to support results from multiple manifest files
            foreach (var result in ProcessManifestFiles(asOf, ManifestFinderRegistry.Finders, fileHistorySource).Take(1))
            {
                yield return result;
                streamWriter.WriteLine(result.ToString());
            }
            streamWriter.Close();
            // TODO: switch to a `using` block
            streamWriter.Dispose();
        }

        private IEnumerable<ScanResult> ProcessManifestFiles(DateTimeOffset asOf, IEnumerable<IManifestFinder> manifestFinders, IFileHistorySource fileHistorySource)
        {
            var resultsCount = 0;

            foreach (var manifestFinder in manifestFinders)
            {
                foreach (var manifestHistory in manifestFinder.GetManifests(fileHistorySource))
                {
                    var analysisDates = new AnalysisDates(manifestHistory, asOf);
                    var metricsResults = analysisDates.Select(
                        ad => ProcessAnalysisDate(manifestHistory.ManifestAsOf(ad), ad)
                    ).ToList();

                    resultsCount++;
                    yield return new ScanResult(manifestHistory.FilePath, metricsResults);

                    // TODO: Remove this break to enable multi-manifest file support, I _think_ ^_^
                    yield break;
                }
            }

            if (resultsCount == 0)
            {
                _logger.LogWarning("Unable to find a manifest file");
            }
        }

        private MetricsResult ProcessAnalysisDate(IManifest manifest, DateTimeOffset analysisDate)
        {
            var calculator = new LibYearCalculator(
                manifest.Repository,
                manifest.Contents,
                manifest.UsesExactMatches,
                _loggerFactory.CreateLogger<LibYearCalculator>()
            );

            var sha = manifest.Revision;

            LibYearResult libYear = calculator.ComputeAsOf(analysisDate);
            _logger.LogTrace(
                "Adding MetricResult: {manifestFile}, " +
                    "currentDate = {currentDate:O}, " +
                    "sha = {sha}, " +
                    "libYear = {ComputeAsOf}",
                manifest.FilePath,
                analysisDate,
                sha,
                libYear.Total
            );

            return new MetricsResult(analysisDate, sha, libYear);
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
