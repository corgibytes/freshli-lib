using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace Freshli.Test.Unit
{
  public class OutputFormatterTest
  {
    [Fact]
    public void Basics()
    {
      var datesAndValues = new List<(DateTime Date, double Value)>()
      {
        (new DateTime(2010, 01, 01), 1.101),
        (new DateTime(2010, 02, 01), 2.202),
        (new DateTime(2010, 03, 01), 3.303),
        (new DateTime(2010, 04, 01), 4.404),
        (new DateTime(2010, 05, 01), 5.505),
        (new DateTime(2010, 06, 01), 6.606),
        (new DateTime(2010, 07, 01), 7.707),
        (new DateTime(2010, 08, 01), 8.808),
        (new DateTime(2010, 09, 01), 9.909),
        (new DateTime(2010, 10, 01), 10.010),
        (new DateTime(2010, 11, 01), 11.111),
        (new DateTime(2010, 12, 01), 12.212)
      };

      var results = new List<MetricsResult>();
      foreach (var dateAndValue in datesAndValues)
      {
        var result = new LibYearResult();
        result.Add(
          "test_package",
          dateAndValue.Value.ToString(),
          dateAndValue.Date,
          dateAndValue.Value);
        results.Add(new MetricsResult(dateAndValue.Date, result));
      }

      var actual = new StringWriter();
      var formatter = new OutputFormatter(actual);
      formatter.Write(results);

      var expected = new StringWriter();
      expected.WriteLine("2010/01/01\t1.101");
      expected.WriteLine("2010/02/01\t2.202");
      expected.WriteLine("2010/03/01\t3.303");
      expected.WriteLine("2010/04/01\t4.404");
      expected.WriteLine("2010/05/01\t5.505");
      expected.WriteLine("2010/06/01\t6.606");
      expected.WriteLine("2010/07/01\t7.707");
      expected.WriteLine("2010/08/01\t8.808");
      expected.WriteLine("2010/09/01\t9.909");
      expected.WriteLine("2010/10/01\t10.010");
      expected.WriteLine("2010/11/01\t11.111");
      expected.WriteLine("2010/12/01\t12.212");

      Assert.Equal(expected.ToString(), actual.ToString());
    }
  }
}
