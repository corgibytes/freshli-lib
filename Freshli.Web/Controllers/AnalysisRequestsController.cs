using System;
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
        var orderedResults = analysisRequest.Results.OrderBy(r => r.Date).
          ToArray();
        var dates = orderedResults.Select(r => r.Date);

        var projectTotalOverTime = Chart.Plot(new Scattergl {
          name = analysisRequest.Url,
          x = dates,
          y = orderedResults.Select(r => r.LibYearResult.Total)
        });
        projectTotalOverTime.WithTitle("Project Total LibYear");

        var projectAverageOverTime = Chart.Plot(new Scattergl {
          name = analysisRequest.Url,
          x = dates,
          y = orderedResults.Select(r =>
            r.LibYearResult.PackageResults.Average(p => p.Value))
        });
        projectAverageOverTime.WithTitle("Project Average LibYear");

        return View(new AnalysisRequestAndResults
        {
          Request = analysisRequest,
          ProjectTotalLibYearOverTime = projectTotalOverTime,
          ProjectAverageLibYearOverTime = projectAverageOverTime
        });

      }

      return View(new AnalysisRequestAndResults
      {
        Request = analysisRequest,
      });
    }
  }
}
