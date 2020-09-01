using System.Threading.Tasks;
using Freshli.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Freshli.Web.Controllers {
  public class AccountController : Controller {

    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;

    public AccountController(UserManager<IdentityUser> userManager,
      SignInManager<IdentityUser> signInManager) {
      _userManager = userManager;
      _signInManager = signInManager;
    }

    public ViewResult Login() {
      return new ViewResult();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel loginModel) {
      if (ModelState.IsValid) {
        var user = await _userManager.FindByNameAsync(loginModel.Name);
        if (user != null) {
          await _signInManager.SignOutAsync();
          var signInResult = await _signInManager.PasswordSignInAsync(user,
            loginModel.Password, isPersistent: false, lockoutOnFailure: false);
          if (signInResult.Succeeded) {
            return Redirect(loginModel.ReturnUrl ?? "/");
          }
        }
      }
      ModelState.AddModelError("", "Invalid name or password");
      return new ViewResult();
    }

    [Authorize]
    public async Task<RedirectResult> Logout(string returnUrl = "/") {
      await _signInManager.SignOutAsync();
      return Redirect(returnUrl);
    }
  }
}
