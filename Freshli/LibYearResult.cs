using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Freshli {
  public class LibYearResult : IEnumerable<LibYearPackageResult> {
    public double Total { get; private set; }

    public int Skipped { get; private set; }

    private List<LibYearPackageResult> _packageResults =
      new List<LibYearPackageResult>();

    public void Add(
      string name,
      string version,
      DateTime publishedAt,
      double value,
      bool skipped
    ) {
      _packageResults.Add(
        new LibYearPackageResult(name, version, publishedAt, value, skipped)
      );
      if (skipped) {
        Skipped++;
      } else {
        Total += value;
      }
    }

    public LibYearPackageResult this[string packageName] {
      get { return _packageResults.Find(item => item.Name == packageName); }
    }

    public IEnumerator<LibYearPackageResult> GetEnumerator() {
      return _packageResults.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() {
      return GetEnumerator();
    }

    public override string ToString() {
      var writer = new StringWriter();
      writer.Write("{ _packagesValues: [ ");
      foreach (var result in this) {
        writer.Write($"{result}, ");
      }

      writer.Write($" ], Total: {Total} }}");

      return writer.ToString();
    }
  }
}
