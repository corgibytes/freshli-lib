using System.Threading.Tasks;
using Freshli.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Freshli.Web.Controllers {
  public class AccountController : Controller {

    private UserManager<IdentityUser> userManager;

    private SignInManager<IdentityUser> signInManager;

    public AccountController(UserManager<IdentityUser> userMgr,
      SignInManager<IdentityUser> signInMgr) {
      userManager = userMgr;
      signInManager = signInMgr;
    }

    public ViewResult Login() {
      return new ViewResult();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel loginModel) {
      if (ModelState.IsValid) {
        IdentityUser user =
          await userManager.FindByNameAsync(loginModel.Name);
        if (user != null) {
          await signInManager.SignOutAsync();
          if ((await signInManager.PasswordSignInAsync(user,
            loginModel.Password, false, false)).Succeeded) {
            return Redirect(loginModel.ReturnUrl ?? "/");
          }
        }
      }
      ModelState.AddModelError("", "Invalid name or password");
      return new ViewResult();
    }

    [Authorize]
    public async Task<RedirectResult> Logout(string returnUrl = "/") {
      await signInManager.SignOutAsync();
      return Redirect(returnUrl);
    }
  }
}
