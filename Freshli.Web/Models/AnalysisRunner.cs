using System;
using System.Collections.Generic;
using Freshli.Web.Data;

namespace Freshli.Web.Models {
  public class AnalysisRunner {
    private ApplicationDbContext _db;

    public AnalysisRunner(ApplicationDbContext db) {
      _db = db;
    }

    public void Run(Guid requestGuid) {
      var analysisRequest = _db.AnalysisRequests.Find(requestGuid);

      ManifestFinder.RegisterAll();
      FileHistoryFinder.Register<GitFileHistoryFinder>();

      var runner = new Runner();
      var results = runner.Run(analysisRequest.Url);

      analysisRequest.Results.AddRange(MapResults(results, analysisRequest));
    }

    private IList<MetricsResult> MapResults(
      IList<Freshli.MetricsResult> rawResults,
      AnalysisRequest request
    ) {
      var persistentResults = new List<MetricsResult>();

      foreach (var rawResult in rawResults) {

        var libYearResult = new LibYearResult {
          Total = rawResult.LibYear.Total
        };
        libYearResult.PackageResults.AddRange(
          MapPackageResults(rawResult.LibYear, libYearResult)
        );

        var metricsResult = new MetricsResult {
          Date = rawResult.Date,
          AnalysisRequest = request,
          LibYearResult = libYearResult
        };

        persistentResults.Add(metricsResult);
      }

      return persistentResults;
    }

    private IList<LibYearPackageResult> MapPackageResults(
      Freshli.LibYearResult rawResult,
      LibYearResult persistentResult
    ) {
      var persistentPackageResults = new List<LibYearPackageResult>();

      foreach (var rawPackageResult in rawResult) {
        var persistentPackageResult = new LibYearPackageResult {
          Name = rawPackageResult.Name,
          Version = rawPackageResult.Version,
          PublishedAt = rawPackageResult.PublishedAt,
          Value = rawPackageResult.Value,
          LibYearResult = persistentResult
        };
        persistentPackageResults.Add(persistentPackageResult);
      }

      return persistentPackageResults;
    }
  }
}
