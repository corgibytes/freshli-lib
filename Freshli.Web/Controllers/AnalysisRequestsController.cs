using System;
using Freshli.Web.Data;
using Freshli.Web.Models;
using Hangfire;
using Microsoft.AspNetCore.Mvc;

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
      return View(analysisRequest);
    }
  }
}
