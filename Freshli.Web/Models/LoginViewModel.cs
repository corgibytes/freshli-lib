using System.ComponentModel.DataAnnotations;

namespace Freshli.Web.Models {
  public class LoginViewModel {

    [Required]
    public string Name { get; set; }

    [Required]
    public string Password { get; set; }

    public string ReturnUrl { get; set; } = "/";
  }
}
