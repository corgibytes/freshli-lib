using System;
using System.Collections.Generic;
using System.Linq;
using Freshli.Web.Data;
using Freshli.Web.Models;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using XPlot.Plotly;

namespace Freshli.Web.Controllers {
  [Route("[controller]")]
  public class AnalysisRequestsController : Controller {
    private ApplicationDbContext _db;

    public AnalysisRequestsController(ApplicationDbContext db) {
      _db = db;
    }

    [HttpPost(Name = "CreateAnalysisRequest")]
    public IActionResult Create(AnalysisRequest analysisRequest) {
      var result = _db.AnalysisRequests.Add(analysisRequest);
      _db.SaveChanges();

      var runner = new AnalysisRunner(_db);
      BackgroundJob.Enqueue(
        () => runner.Run(analysisRequest.Id)
      );

      return RedirectToRoute(
        "ShowAnalysisRequest",
        new {id = analysisRequest.Id}
      );
    }

    [HttpGet("{id}", Name = "ShowAnalysisRequest")]
    public IActionResult Show(Guid id) {
      var analysisRequest = _db.AnalysisRequests.Find(id);

      if (analysisRequest.Results != null)
      {
        var orderedResults = analysisRequest.Results.
          OrderBy(r => r.Date).ToArray();
        var dates = orderedResults.Select(r => r.Date);

        var projectTotalOverTime = ProjectTotalOverTime(
          analysisRequest,
          dates,
          orderedResults
        );
        var projectAverageOverTime = ProjectAverageOverTime(
          analysisRequest,
          dates,
          orderedResults
        );
        var projectMaxOverTime = ProjectMaxOverTime(
          analysisRequest,
          dates,
          orderedResults
        );

        var names = orderedResults.Select(
          r => r.LibYearResult.PackageResults.
            Select(p => p.Name).ToArray()).Aggregate(
          new HashSet<string>(),
          (hash, values) => {
            foreach(var value in values) {
              hash.Add(value);
            }
            return hash;
          }
        ).OrderBy(name => name);

        var areaSeries = new List<Scatter>();

        foreach (var name  in names) {
          var projectData = orderedResults.
            Where(r => r.LibYearResult.PackageResults.
              Any(p => p.Name == name));
          var projectDates = projectData.Select(r => r.Date);
          var projectLibYearValues = projectData.
            Select(r => r.LibYearResult.PackageResults.
              First(p => p.Name == name).Value);

          areaSeries.Add(new Scatter {
            name = name,
            x = projectDates,
            y = projectLibYearValues
          });
        }

        var chart = Chart.Plot(areaSeries.ToArray());
        chart.WithLayout(new Layout.Layout {
          title = "LibYear over time per dependency",
          hovermode = "closest"
        });

        return View(new AnalysisRequestAndResults
        {
          Request = analysisRequest,
          ProjectTotalLibYearOverTime = projectTotalOverTime,
          ProjectAverageLibYearOverTime = projectAverageOverTime,
          ProjectMaxLibYearOverTime = projectMaxOverTime,
          DependenciesLibYearOverTimeStacked = chart
        });

      }

      return View(new AnalysisRequestAndResults
      {
        Request = analysisRequest,
      });
    }

    private static PlotlyChart ProjectMaxOverTime(
      AnalysisRequest analysisRequest,
      IEnumerable<DateTime> dates,
      Models.MetricsResult[] orderedResults
    ) {
      var projectMaxOverTime = Chart.Plot(
        new Scattergl {
          name = analysisRequest.Url,
          x = dates,
          y = orderedResults.Select(
            r =>
              r.LibYearResult.PackageResults.Max(p => p.Value)
          )
        }
      );
      projectMaxOverTime.WithTitle("Project Max LibYear");
      return projectMaxOverTime;
    }

    private static PlotlyChart ProjectAverageOverTime(
      AnalysisRequest analysisRequest,
      IEnumerable<DateTime> dates,
      Models.MetricsResult[] orderedResults
    ) {
      var projectAverageOverTime = Chart.Plot(
        new Scattergl {
          name = analysisRequest.Url,
          x = dates,
          y = orderedResults.Select(
            r =>
              r.LibYearResult.PackageResults.Average(p => p.Value)
          )
        }
      );
      projectAverageOverTime.WithTitle("Project Average LibYear");
      return projectAverageOverTime;
    }

    private static PlotlyChart ProjectTotalOverTime(
      AnalysisRequest analysisRequest,
      IEnumerable<DateTime> dates,
      Models.MetricsResult[] orderedResults
    ) {
      var projectTotalOverTime = Chart.Plot(
        new Scattergl
        {
          name = analysisRequest.Url,
          x = dates,
          y = orderedResults.Select(r => r.LibYearResult.Total)
        });
      projectTotalOverTime.WithTitle("Project Total LibYear");
      return projectTotalOverTime;
    }
  }
}
