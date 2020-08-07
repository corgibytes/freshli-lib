using System;
using System.Collections.Generic;
using System.Data;
using Freshli.Web.Data;

namespace Freshli.Web.Models {
  public class AnalysisRunner {
    private ApplicationDbContext _db;

    public AnalysisRunner(ApplicationDbContext db) {
      _db = db;
    }

    private void SetStartingState(AnalysisRequest request) {
      if (request.State == AnalysisRequest.Status.New)
      {
        request.State = AnalysisRequest.Status.InProgress;
      }
      else
      {
        request.State = AnalysisRequest.Status.Retrying;
      }

      _db.Update(request);
      _db.SaveChanges();
    }

    public void Run(Guid requestGuid) {
      var analysisRequest = _db.AnalysisRequests.Find(requestGuid);
      SetStartingState(analysisRequest);

      ManifestFinder.RegisterAll();
      FileHistoryFinder.Register<GitFileHistoryFinder>();

      var runner = new Runner();
      var results = runner.Run(analysisRequest.Url);

      if (analysisRequest.Results == null) {
        analysisRequest.Results = new List<MetricsResult>();
      } else {
        _db.MetricsResults.RemoveRange(analysisRequest.Results);
        analysisRequest.Results.Clear();
      }

      analysisRequest.Results.AddRange(MapResults(results, analysisRequest));
      analysisRequest.State = AnalysisRequest.Status.Success;
      _db.Update(analysisRequest);

      _db.SaveChanges();
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
        libYearResult.PackageResults = new List<LibYearPackageResult>();
        libYearResult.PackageResults.AddRange(
          MapPackageResults(rawResult.LibYear, libYearResult)
        );
        _db.LibYearResults.AddAsync(libYearResult);

        var metricsResult = new MetricsResult {
          Date = rawResult.Date,
          AnalysisRequest = request,
          LibYearResult = libYearResult,
          LibYearResultId = libYearResult.Id
        };
        _db.MetricsResults.AddAsync(metricsResult);

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
        _db.LibYearPackageResults.AddAsync(persistentPackageResult);
        persistentPackageResults.Add(persistentPackageResult);
      }

      return persistentPackageResults;
    }
  }
}
