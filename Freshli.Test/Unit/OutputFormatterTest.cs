using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace Freshli.Test.Unit {
  public class OutputFormatterTest {
    [Fact]
    public void Basics() {
      var datesAndValues = new List<(DateTime Date, double Value, bool Skipped)>() {
        (new DateTime(2010, 01, 01), 1.101, false),
        (new DateTime(2010, 02, 01), 2.202, false),
        (new DateTime(2010, 03, 01), 3.303, false),
        (new DateTime(2010, 04, 01), 4.404, false),
        (new DateTime(2010, 05, 01), 5.505, false),
        (new DateTime(2010, 06, 01), 6.606, false),
        (new DateTime(2010, 07, 01), 7.707, false),
        (new DateTime(2010, 08, 01), 8.808, false),
        (new DateTime(2010, 09, 01), 9.909, false),
        (new DateTime(2010, 10, 01), 10.010, false),
        (new DateTime(2010, 11, 01), 11.111, false),
        (new DateTime(2010, 12, 01), 12.212, false)
      };

      var results = new List<MetricsResult>();
      foreach (var dateAndValue in datesAndValues) {
        var result = new LibYearResult();
        result.Add(
          "test_package",
          dateAndValue.Value.ToString(),
          dateAndValue.Date,
          dateAndValue.Value,
          dateAndValue.Skipped
        );
        results.Add(new MetricsResult(dateAndValue.Date, result));
      }

      var actual = new StringWriter();
      var formatter = new OutputFormatter(actual);
      formatter.Write(results);

      var expected = new StringWriter();
      expected.WriteLine("Date\tLibYear\tSkipped");
      expected.WriteLine("2010/01/01\t1.101\t0");
      expected.WriteLine("2010/02/01\t2.202\t0");
      expected.WriteLine("2010/03/01\t3.303\t0");
      expected.WriteLine("2010/04/01\t4.404\t0");
      expected.WriteLine("2010/05/01\t5.505\t0");
      expected.WriteLine("2010/06/01\t6.606\t0");
      expected.WriteLine("2010/07/01\t7.707\t0");
      expected.WriteLine("2010/08/01\t8.808\t0");
      expected.WriteLine("2010/09/01\t9.909\t0");
      expected.WriteLine("2010/10/01\t10.010\t0");
      expected.WriteLine("2010/11/01\t11.111\t0");
      expected.WriteLine("2010/12/01\t12.212\t0");

      Assert.Equal(expected.ToString(), actual.ToString());
    }
  }
}
