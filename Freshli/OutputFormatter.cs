using System.Collections.Generic;
using System.IO;

namespace Freshli
{
  public class OutputFormatter
  {
    private readonly TextWriter _writer;

    public OutputFormatter(TextWriter writer)
    {
      _writer = writer;
    }

    public void Write(IList<MetricsResult> results)
    {
      foreach (var resultSet in results)
      {
        _writer.WriteLine($"{resultSet.Date.ToString("yyyy/MM/dd")}\t" +
                          $"{resultSet.LibYear.Total.ToString("F3")}");
      }
    }
  }
}
