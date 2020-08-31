using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Freshli.Web.Data;

namespace Freshli.Web.Models {
  public static class IdentitySeedData {
    private static readonly string AdminUserEmail =
      Environment.GetEnvironmentVariable("ADMIN_EMAIL") ?? "admin@freshli.io";
    private static readonly string AdminUsername =
      Environment.GetEnvironmentVariable("ADMIN_USERNAME") ?? "admin";
    private static readonly string AdminPassword =
      Environment.GetEnvironmentVariable("ADMIN_PASSWORD") ?? "Secret123$";

    public static async void EnsurePopulated(IApplicationBuilder app) {
      ApplicationDbContext context = app.ApplicationServices
        .CreateScope().ServiceProvider
        .GetRequiredService<ApplicationDbContext>();
      if (context.Database.GetPendingMigrations().Any()) {
        context.Database.Migrate();
      }
      UserManager<IdentityUser> userManager = app.ApplicationServices
        .CreateScope().ServiceProvider
        .GetRequiredService<UserManager<IdentityUser>>();
      IdentityUser user = await userManager.FindByIdAsync(AdminUsername);
      if (user == null) {
        user = new IdentityUser(AdminUsername);
        user.Email = AdminUserEmail;
        await userManager.CreateAsync(user, AdminPassword);
      }
    }
  }
}
