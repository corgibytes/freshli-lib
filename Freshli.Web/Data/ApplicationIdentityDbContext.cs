using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Freshli.Web.Data {
  public class ApplicationIdentityDbContext : IdentityDbContext<IdentityUser> {
    public ApplicationIdentityDbContext(
      DbContextOptions<ApplicationIdentityDbContext> options)
      : base(options) { }
  }
}
