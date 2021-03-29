using System;
using Corgibytes.Freshli.Lib.Util;
using Xunit;

namespace Corgibytes.Freshli.Lib.Test.Unit.Util {
  public class DateTimeExtensionsTest {
    [Theory]
    [InlineData(
      new[] {2020, 1, 5, 23, 59, 59, 999, 9998},
      new[] {2020, 1, 6, 23, 59, 59, 999, 9999},
      -1
    )]
    [InlineData(
      new[] {2020, 1, 5, 23, 59, 59, 999, 9999},
      new[] {2020, 1, 6, 23, 59, 59, 999, 9999},
      -1
    )]
    [InlineData(
      new[] {2020, 1, 6, 0, 0, 0, 0, 0},
      new[] {2020, 1, 6, 23, 59, 59, 999, 9999},
      0
    )]
    [InlineData(
      new[] {2020, 1, 6, 0, 0, 0, 0, 1},
      new[] {2020, 1, 6, 23, 59, 59, 999, 9999},
      0
    )]
    [InlineData(
      new[] {2020, 1, 6, 12, 0, 0, 0, 0},
      new[] {2020, 1, 6, 23, 59, 59, 999, 9999},
      0
    )]
    [InlineData(
      new[] {2020, 1, 6, 23, 59, 59, 999, 9998},
      new[] {2020, 1, 6, 23, 59, 59, 999, 9999},
      0
    )]
    [InlineData(
      new[] {2020, 1, 6, 23, 59, 59, 999, 9999},
      new[] {2020, 1, 6, 23, 59, 59, 999, 9999},
      0
    )]
    [InlineData(
      new[] {2020, 1, 7, 0, 0, 0, 0, 0},
      new[] {2020, 1, 6, 23, 59, 59, 999, 9999},
      1
    )]
    [InlineData(
      new[] {2020, 1, 7, 0, 0, 0, 0, 1},
      new[] {2020, 1, 6, 23, 59, 59, 999, 9999},
      1
    )]
    public void EndOfDayValuesCompareCorrectly(
      int[] dateArguments,
      int[] targetDateArguments,
      int comparison
    ) {
      AssertDateExtensionMethod(
        dateArguments,
        targetDateArguments,
        comparison,
        date => date.ToEndOfDay()
      );
    }

    [Theory]
    [InlineData(
      new[] {2020, 1, 5, 23, 59, 59, 999, 9998},
      new[] {2020, 1, 6, 00, 00, 00, 000, 0000},
      -1
    )]
    [InlineData(
      new[] {2020, 1, 5, 23, 59, 59, 999, 9999},
      new[] {2020, 1, 6, 00, 00, 00, 000, 0000},
      -1
    )]
    [InlineData(
      new[] {2020, 1, 6, 0, 0, 0, 0, 0},
      new[] {2020, 1, 6, 00, 00, 00, 000, 0000},
      0
    )]
    [InlineData(
      new[] {2020, 1, 6, 0, 0, 0, 0, 1},
      new[] {2020, 1, 6, 00, 00, 00, 000, 0000},
      0
    )]
    [InlineData(
      new[] {2020, 1, 6, 12, 0, 0, 0, 0},
      new[] {2020, 1, 6, 00, 00, 00, 000, 0000},
      0
    )]
    [InlineData(
      new[] {2020, 1, 6, 23, 59, 59, 999, 9998},
      new[] {2020, 1, 6, 00, 00, 00, 000, 0000},
      0
    )]
    [InlineData(
      new[] {2020, 1, 6, 23, 59, 59, 999, 9999},
      new[] {2020, 1, 6, 00, 00, 00, 000, 0000},
      0
    )]
    [InlineData(
      new[] {2020, 1, 7, 0, 0, 0, 0, 0},
      new[] {2020, 1, 6, 00, 00, 00, 000, 0000},
      1
    )]
    [InlineData(
      new[] {2020, 1, 7, 0, 0, 0, 0, 1},
      new[] {2020, 1, 6, 00, 00, 00, 000, 0000},
      1
    )]
    public void StartOfDayValuesCompareCorrectly(
      int[] dateArguments,
      int[] targetDateArguments,
      int comparison
    ) {
      AssertDateExtensionMethod(
        dateArguments,
        targetDateArguments,
        comparison,
        date => date.ToStartOfDay()
      );
    }

    [Theory]
    [InlineData(
      new[] {2019, 12, 05, 23, 59, 59, 999, 9998},
      new[] {2020, 01, 01, 00, 00, 00, 000, 0000},
      -1
    )]
    [InlineData(
      new[] {2020, 01, 05, 23, 59, 59, 999, 9998},
      new[] {2020, 01, 01, 00, 00, 00, 000, 0000},
      0
    )]
    [InlineData(
      new[] {2020, 01, 05, 23, 59, 59, 999, 9999},
      new[] {2020, 01, 01, 00, 00, 00, 000, 0000},
      0
    )]
    [InlineData(
      new[] {2020, 1, 6, 0, 0, 0, 0, 0},
      new[] {2020, 01, 01, 00, 00, 00, 000, 0000},
      0
    )]
    [InlineData(
      new[] {2020, 1, 6, 0, 0, 0, 0, 1},
      new[] {2020, 01, 01, 00, 00, 00, 000, 0000},
      0
    )]
    [InlineData(
      new[] {2020, 1, 6, 12, 0, 0, 0, 0},
      new[] {2020, 01, 01, 00, 00, 00, 000, 0000},
      0
    )]
    [InlineData(
      new[] {2020, 1, 6, 23, 59, 59, 999, 9998},
      new[] {2020, 01, 01, 00, 00, 00, 000, 0000},
      0
    )]
    [InlineData(
      new[] {2020, 1, 6, 23, 59, 59, 999, 9999},
      new[] {2020, 01, 01, 00, 00, 00, 000, 0000},
      0
    )]
    [InlineData(
      new[] {2020, 1, 7, 0, 0, 0, 0, 0},
      new[] {2020, 01, 01, 00, 00, 00, 000, 0000},
      0
    )]
    [InlineData(
      new[] {2020, 1, 7, 0, 0, 0, 0, 1},
      new[] {2020, 01, 01, 00, 00, 00, 000, 0000},
      0
    )]
    [InlineData(
      new[] {2020, 2, 7, 0, 0, 0, 0, 0},
      new[] {2020, 01, 01, 00, 00, 00, 000, 0000},
      1
    )]
    [InlineData(
      new[] {2020, 2, 7, 0, 0, 0, 0, 1},
      new[] {2020, 01, 01, 00, 00, 00, 000, 0000},
      1
    )]
    public void StartOfMonthValuesCompareCorrectly(
      int[] dateArguments,
      int[] targetDateArguments,
      int comparison
    ) {
      AssertDateExtensionMethod(
        dateArguments,
        targetDateArguments,
        comparison,
        date => date.ToStartOfMonth()
      );
    }

    private static DateTimeOffset BuildDateTimeOffset(int[] dateArguments)
    {
      var inputDate = new DateTimeOffset(
        dateArguments[0],
        dateArguments[1],
        dateArguments[2],
        dateArguments[3],
        dateArguments[4],
        dateArguments[5],
        dateArguments[6],
        TimeSpan.Zero
      ).AddTicks(dateArguments[7]);
      return inputDate;
    }

    private void AssertDateExtensionMethod(
      int[] dateArguments,
      int[] targetDateArguments,
      int comparison,
      Func<DateTimeOffset, DateTimeOffset> extensionMethod
    ) {
      var inputDate = BuildDateTimeOffset(dateArguments);
      var targetDate = BuildDateTimeOffset(targetDateArguments);

      var transformedDate = extensionMethod(inputDate);

      Assert.Equal(comparison, transformedDate.CompareTo(targetDate));
    }
  }
}
