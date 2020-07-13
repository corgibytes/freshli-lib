using Freshli.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Freshli.Web.Data {
  public class ApplicationDbContext: DbContext {
    public DbSet<AnalysisRequest> AnalysisRequests { get; set; }

    public ApplicationDbContext(
      DbContextOptions<ApplicationDbContext> options
    ): base(options) {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      base.OnModelCreating(modelBuilder);

      modelBuilder.Entity<AnalysisRequest>();
    }
  }
}
