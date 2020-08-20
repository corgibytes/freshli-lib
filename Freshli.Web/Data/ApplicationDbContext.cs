using System;
using Freshli.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Freshli.Web.Data {
  public class ApplicationDbContext : DbContext {
    public DbSet<AnalysisRequest> AnalysisRequests { get; set; }

    public DbSet<Models.LibYearPackageResult> LibYearPackageResults {
      get;
      set;
    }

    public DbSet<Models.LibYearResult> LibYearResults { get; set; }
    public DbSet<Models.MetricsResult> MetricsResults { get; set; }

    public ApplicationDbContext(
      DbContextOptions<ApplicationDbContext> options
    ) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      base.OnModelCreating(modelBuilder);

      modelBuilder.Entity<AnalysisRequest>().
        Property(m => m.State).
        HasConversion(
          v => v.ToString(),
          v => (AnalysisRequestStatus)Enum.Parse(
            typeof(AnalysisRequestStatus),
            v
          )
        );
      modelBuilder.Entity<Models.LibYearPackageResult>();
      modelBuilder.Entity<Models.LibYearResult>();
      modelBuilder.Entity<Models.MetricsResult>().
        HasOne(m => m.LibYearResult).
        WithOne(m => m.MetricsResult).
        HasForeignKey<Models.LibYearResult>(m => m.MetricsResultId);
    }
  }
}
