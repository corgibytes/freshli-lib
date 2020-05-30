using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace LibMetrics
{
  public class LibYearResult: IEnumerable<LibYearPackageResult>
  {
    public double Total { get; private set; }

    private List<LibYearPackageResult> _packageValues =
      new List<LibYearPackageResult>();

    public void Add(string name, string version, DateTime publishedAt, double value)
    {
      _packageValues.Add(new LibYearPackageResult(name, version, publishedAt, value));
      Total += value;
    }

    public LibYearPackageResult this[string packageName]
    {
      get { return _packageValues.Find(item => item.Name == packageName); }
    }

    public IEnumerator<LibYearPackageResult> GetEnumerator()
    {
      return _packageValues.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    public override string ToString()
    {
      var writer = new StringWriter();
      writer.Write("{ _packagesValues: [ ");
      foreach (var result in this)
      {
        writer.Write($"{result}, ");
      }
      writer.Write($" ], Total: {Total} }}");

      return writer.ToString();
    }
  }

}
