using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Freshli.Web.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Freshli.Web.Models;
using Hangfire;

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
