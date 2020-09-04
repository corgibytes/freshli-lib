using System;
using Freshli.Web.Data;
using Freshli.Web.Models;
using Hangfire;
using Hangfire.Annotations;
using Hangfire.Dashboard;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Freshli.Web {
  public class Startup {
    public Startup(IConfiguration configuration) {
      Configuration = configuration;
      DotNetEnv.Env.Load();
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
      var sslMode = Environment.GetEnvironmentVariable("DB_SSL_MODE") ??
        "Disable";

      var connectionString = $"Host={host};Database={database};" +
        $"Username={username};Password={password};SSL Mode={sslMode};";

      services.AddControllersWithViews();

      services.AddDbContext<ApplicationDbContext>(
        options =>
          options.
            UseLazyLoadingProxies().
            UseNpgsql(connectionString).
            UseSnakeCaseNamingConvention()
      );

      services.AddIdentity<IdentityUser, IdentityRole>()
        .AddEntityFrameworkStores<ApplicationDbContext>();

      services.AddHangfire(configuration => configuration.
        SetDataCompatibilityLevel(CompatibilityLevel.Version_170).
        UseSimpleAssemblyNameTypeSerializer().
        UsePostgreSqlStorage(connectionString)
      );

      services.AddMvc();
    }

    // This method gets called by the runtime. Use this method to configure
    // the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
      if (env.IsDevelopment()) {
        app.UseDeveloperExceptionPage();
      } else {
        app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change
        // this for production scenarios,
        // see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
      }

      app.UseHttpsRedirection();
      app.UseStaticFiles();

      app.UseRouting();

      app.UseAuthentication();
      app.UseAuthorization();

      app.UseHangfireDashboard("/admin/jobs", new DashboardOptions {
        Authorization = new[] { new HangfireAuthorizationFilter() }
      });

      app.UseEndpoints(
        endpoints => {
          endpoints.MapControllers();
          endpoints.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}"
          );
        }
      );

      IdentitySeedData.EnsurePopulated(app);
    }
  }

  public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter {
    public bool Authorize([NotNull] DashboardContext context) {
      var httpContext = context.GetHttpContext();
      if(!httpContext.User.Identity.IsAuthenticated) {
        httpContext.Response.Redirect("/account/login");
      }
      return true;
    }
  }
}
