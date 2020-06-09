using System.Collections.Generic;
using System.IO;

namespace Freshli.Test
{
  public class Fixtures
  {
    public static string Path(params string[] values)
    {
      var assemblyPath = System.Reflection.Assembly.
        GetExecutingAssembly().Location;

      var components = new List<string>()
      {
        Directory.GetParent(assemblyPath).ToString(),
        "fixtures"
      };
      components.AddRange(values);

      return System.IO.Path.Combine(components.ToArray());
    }
  }
}
