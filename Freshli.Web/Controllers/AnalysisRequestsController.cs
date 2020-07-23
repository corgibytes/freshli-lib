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
        var orderedResults = analysisRequest.Results.OrderBy(r => r.Date);
        var lineSeries = new Scattergl {
          name = analysisRequest.Url,
          x = orderedResults.Select(r => r.Date),
          y = orderedResults.Select(r => r.LibYearResult.Total)
        };

        var chart = Chart.Plot(lineSeries);
        chart.WithTitle("LibYear over time");

        return View(new AnalysisRequestAndResults
        {
          Request = analysisRequest,
          TotalLineChart = chart
        });

      }

      return View(new AnalysisRequestAndResults
      {
        Request = analysisRequest,
      });
    }
  }
}
