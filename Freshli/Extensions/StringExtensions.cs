using System.Text.RegularExpressions;

namespace Freshli.Extensions {
  public static class StringExtensions {
    public static string GetVersionRequirement(this string value) {
      string pattern = @"[<>=!~]{1,2}";
      Match m = Regex.Match(value, pattern);
      return m.Value;
    }
  }
}
