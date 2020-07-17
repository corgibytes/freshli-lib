using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Freshli.Web.Worker {
  public class Program {
    public static void Main(string[] args) {
      CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) {
      var host = Environment.GetEnvironmentVariable("DB_HOST") ??
        "localhost";
      var username = Environment.GetEnvironmentVariable("DB_USERNAME") ??
        "postgres";
      var password = Environment.GetEnvironmentVariable("DB_PASSWORD") ??
        "password";
      var database = Environment.GetEnvironmentVariable("DB_NAME") ??
        "freshli_web_development";

      var connectionString = $"Host={host};Database={database};" +
        $"Username={username};Password={password};";

      return Host.CreateDefaultBuilder(args).ConfigureServices(
        (hostContext, services) => {
          services.AddHostedService<Worker>();
          services.AddDbContext<ApplicationDbContext>(
            options =>
              options.
                UseNpgsql(connectionString).
                UseSnakeCaseNamingConvention()
          );
          services.AddHangfireServer();
        }
      );
    }
  }
}
