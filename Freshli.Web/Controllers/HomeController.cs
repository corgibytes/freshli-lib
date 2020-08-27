using System.Diagnostics;
using Freshli.Web.Data;
using Freshli.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Freshli.Web.Controllers {
  public class HomeController : Controller {
    private readonly ILogger<HomeController> _logger;
    private ApplicationDbContext _db;

    public HomeController(
      ILogger<HomeController> logger,
      ApplicationDbContext db
    ) {
      _logger = logger;
      _db = db;
    }

    public IActionResult Index() {
      return View("~/Views/AnalysisRequests/Create.cshtml");
    }

    public IActionResult Terms() {
      return View();
    }

    public IActionResult Privacy() {
      return View();
    }

    [ResponseCache(
      Duration = 0,
      Location = ResponseCacheLocation.None,
      NoStore = true
    )]
    public IActionResult Error() {
      return View(
        new ErrorViewModel {
          RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
        }
      );
    }
  }
}
