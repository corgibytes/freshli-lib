using System.Collections.Generic;
using System.IO;

namespace LibMetrics.Test
{
  public class Helpers
  {
    public static string FixturePath(params string[] values)
    {
      var assemblyPath = System.Reflection.Assembly.
        GetExecutingAssembly().Location;

      var components = new List<string>()
      {
        Directory.GetParent(assemblyPath).ToString(),
        "fixtures"
      };
      components.AddRange(values);

      return Path.Combine(components.ToArray());
    }
  }
}
