using System;
using System.Collections.Generic;
using System.IO;
using Freshli.cli;
using Xunit;

namespace Freshli.Test.cli {
  public class OutputFormatterTest {
    [Fact]
    public void Basics() {
      var datesAndValues = new List<(
        DateTime Date, double Value, bool UpgradeAvailable, bool Skipped)>
      {
        (new DateTime(2010, 01, 01), 1.1010, false, false),
        (new DateTime(2010, 02, 01), 2.2020, false, false),
        (new DateTime(2010, 03, 01), 3.3030, false, false),
        (new DateTime(2010, 04, 01), 4.4040, false, false),
        (new DateTime(2010, 05, 01), 5.5050, false, false),
        (new DateTime(2010, 06, 01), 6.6060, false, false),
        (new DateTime(2010, 07, 01), 7.7070, false, false),
        (new DateTime(2010, 08, 01), 8.8080, false, false),
        (new DateTime(2010, 09, 01), 9.9090, false, false),
        (new DateTime(2010, 10, 01), 10.0101, false, false),
        (new DateTime(2010, 11, 01), 11.1111, true, false),
        (new DateTime(2010, 12, 01), 12.2121, true, false),
        (new DateTime(2011, 01, 01), 13.3333, false, true)
      };

      var results = new List<MetricsResult>();
      foreach (var dateAndValue in datesAndValues) {
        var result = new LibYearResult();
        result.Add(
          "test_package",
          "1.0",
          dateAndValue.Date,
          "2.0",
          DateTime.Today,
          dateAndValue.Value,
          dateAndValue.UpgradeAvailable,
          dateAndValue.Skipped
        );
        results.Add(new MetricsResult(dateAndValue.Date, "N/A", result));
      }

      var actual = new StringWriter();
      var formatter = new OutputFormatter(actual);
      formatter.Write(results);

      var expected = new StringWriter();
      expected.WriteLine("Date\tLibYear\tUpgradesAvailable\tSkipped");
      expected.WriteLine("2010/01/01\t1.1010\t0\t0");
      expected.WriteLine("2010/02/01\t2.2020\t0\t0");
      expected.WriteLine("2010/03/01\t3.3030\t0\t0");
      expected.WriteLine("2010/04/01\t4.4040\t0\t0");
      expected.WriteLine("2010/05/01\t5.5050\t0\t0");
      expected.WriteLine("2010/06/01\t6.6060\t0\t0");
      expected.WriteLine("2010/07/01\t7.7070\t0\t0");
      expected.WriteLine("2010/08/01\t8.8080\t0\t0");
      expected.WriteLine("2010/09/01\t9.9090\t0\t0");
      expected.WriteLine("2010/10/01\t10.0101\t0\t0");
      expected.WriteLine("2010/11/01\t11.1111\t1\t0");
      expected.WriteLine("2010/12/01\t12.2121\t1\t0");
      expected.WriteLine("2011/01/01\t0.0000\t0\t1");

      Assert.Equal(expected.ToString(), actual.ToString());
    }
  }
}
