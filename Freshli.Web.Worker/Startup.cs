using System;
using Freshli.Web.Data;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Freshli.Web.Worker {
  public class Startup {
    public Startup(IConfiguration configuration) {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add
    // services to the container.
    public void ConfigureServices(IServiceCollection services) {
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

      services.AddControllersWithViews();
      services.AddDbContext<ApplicationDbContext>(
        options =>
          options.
            UseNpgsql(connectionString).
            UseSnakeCaseNamingConvention()
      );
      services.AddHangfire(configuration => configuration.
        SetDataCompatibilityLevel(CompatibilityLevel.Version_170).
        UseSimpleAssemblyNameTypeSerializer().
        UsePostgreSqlStorage(connectionString)
      );
    }

    // This method gets called by the runtime. Use this method to configure
    // the HTTP request pipeline.
    public void Configure(IApplicationBuilder app) {
      app.UseHangfireServer();
    }
  }
}
