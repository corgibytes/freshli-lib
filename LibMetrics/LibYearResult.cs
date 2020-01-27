using System;
using System.Collections;
using System.Collections.Generic;

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
  }

}
