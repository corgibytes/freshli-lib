using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using Corgibytes.Freshli.Lib.Util;

namespace Corgibytes.Freshli.Lib
{
    public class Runner : IRunner
    {
        private const string ResultsPath = "results";

        public ILogger<Runner> Logger { get; }

        public IManifestService ManifestService { get; }
        public IFileHistoryService FileHistoryService { get; }
        public ICalculatorService CalculatorService { get; }
        public IAnalysisDatesService AnalysisDatesService { get; }

        public Runner(ILogger<Runner> logger, IManifestService manifestService, IFileHistoryService fileHistoryService, ICalculatorService calculatorService, IAnalysisDatesService analysisDatesService)
        {
            Logger = logger;
            ManifestService = manifestService;
            FileHistoryService = fileHistoryService;
            CalculatorService = calculatorService;
            AnalysisDatesService = analysisDatesService;
        }

        public IList<ScanResult> Run(string analysisPath, DateTime asOf)
        {
            Logger.LogInformation($"Run({analysisPath}, {asOf:d})");

            IList<ScanResult> scanResults = new List<ScanResult>();

            var fileHistoryFinder = FileHistoryService.SelectFinderFor(analysisPath);
            var manifestFinders = ManifestService.SelectFindersFor(analysisPath, fileHistoryFinder);

            if (!manifestFinders.Any(finder => finder.Successful))
            {
                Logger.LogWarning("Unable to find a manifest file");
            }
            else
            {
                scanResults = ProcessManifestFiles(
                    manifestFinders,
                    fileHistoryFinder,
                    analysisPath,
                    asOf
                );
            }

            return scanResults;
        }

        public IList<ScanResult> Run(string analysisPath)
        {
            var asOf = DateTime.Today.ToEndOfDay();
            return Run(analysisPath, asOf: asOf);
        }

        // TODO: Create an async version of this method
        private IList<ScanResult> ProcessManifestFiles(IEnumerable<IManifestFinder> manifestFinders, IFileHistoryFinder fileHistoryFinder, string analysisPath, DateTime asOf)
        {
            var scanResults = new List<ScanResult>();
            foreach (var manifestFinder in manifestFinders)
            {
                foreach (var manifestFile in manifestFinder.ManifestFiles)
                {
                    Logger.LogTrace(
                        "{analysisPath}: LockFileName: {LockFileName}",
                        analysisPath,
                        manifestFile
                        );

                    var fileHistory = fileHistoryFinder.FileHistoryOf(manifestFile);
                    var analysisDates = AnalysisDatesService.DetermineDatesFor(fileHistory, asOf);
                    var metricsResults = CalculatorService.Compute(fileHistory, analysisDates);

                    scanResults.Add(new ScanResult(manifestFile, metricsResults));
                }
            }

            return scanResults;
        }

        // TODO: Move this to the CalculatorService class to be called from the #Compute method
        private MetricsResult ProcessAnalysisDate(string manifestFile, LibYearCalculator calculator, IFileHistory fileHistory, DateTime currentDate)
        {
            var content = fileHistory.ContentsAsOf(currentDate);
            calculator.Manifest.Parse(content);

            var sha = fileHistory.ShaAsOf(currentDate);

            Task<LibYearResult> libYear = calculator.ComputeAsOf(currentDate);
            Logger.LogTrace(
                "Adding MetricResult: {manifestFile}, " +
                "currentDate = {currentDate:d}, " +
                "sha = {sha}, " +
                "libYear = {ComputeAsOf}",
                manifestFile,
                currentDate,
                sha,
                libYear.Result.Total
            );

            return new MetricsResult(currentDate, sha, libYear.Result);
        }
    }
}
