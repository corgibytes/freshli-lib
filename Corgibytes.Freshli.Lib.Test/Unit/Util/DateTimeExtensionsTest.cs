using System;
using Corgibytes.Freshli.Lib.Util;
using Xunit;

namespace Corgibytes.Freshli.Lib.Test.Unit.Util {
  public class DateTimeExtensionsTest {
    [Theory]
    [InlineData(new[] {2020, 1, 5, 23, 59, 59, 999, 9998}, -1)]
    [InlineData(new[] {2020, 1, 5, 23, 59, 59, 999, 9999}, -1)]
    [InlineData(new[] {2020, 1, 6, 0, 0, 0, 0, 0}, 0)]
    [InlineData(new[] {2020, 1, 6, 0, 0, 0, 0, 1}, 0)]
    [InlineData(new[] {2020, 1, 6, 12, 0, 0, 0, 0}, 0)]
    [InlineData(new[] {2020, 1, 6, 23, 59, 59, 999, 9998}, 0)]
    [InlineData(new[] {2020, 1, 6, 23, 59, 59, 999, 9999}, 0)]
    [InlineData(new[] {2020, 1, 7, 0, 0, 0, 0, 0}, 1)]
    [InlineData(new[] {2020, 1, 7, 0, 0, 0, 0, 1}, 1)]
    public void EndOfDayValuesCompareCorrectly(
      int[] dateArguments,
      int comparison
    ) {
      var inputDate = BuildDateTimeOffset(dateArguments);

      var endOfDayOn2020106 =
        new DateTimeOffset(2020, 1, 6, 23, 59, 59, 999, TimeSpan.Zero).
          AddTicks(9999);

      var inputDateEndOfDay = inputDate.ToEndOfDay();

      Assert.Equal(
        comparison,
        inputDateEndOfDay.CompareTo(endOfDayOn2020106)
      );
    }

    [Theory]
    [InlineData(new[] {2020, 1, 5, 23, 59, 59, 999, 9998}, -1)]
    [InlineData(new[] {2020, 1, 5, 23, 59, 59, 999, 9999}, -1)]
    [InlineData(new[] {2020, 1, 6, 0, 0, 0, 0, 0}, 0)]
    [InlineData(new[] {2020, 1, 6, 0, 0, 0, 0, 1}, 0)]
    [InlineData(new[] {2020, 1, 6, 12, 0, 0, 0, 0}, 0)]
    [InlineData(new[] {2020, 1, 6, 23, 59, 59, 999, 9998}, 0)]
    [InlineData(new[] {2020, 1, 6, 23, 59, 59, 999, 9999}, 0)]
    [InlineData(new[] {2020, 1, 7, 0, 0, 0, 0, 0}, 1)]
    [InlineData(new[] {2020, 1, 7, 0, 0, 0, 0, 1}, 1)]
    public void StartOfDayValuesCompareCorrectly(
      int[] dateArguments,
      int comparison
    ) {
      var inputDate = BuildDateTimeOffset(dateArguments);

      var startOfDayOn2020106 =
        new DateTimeOffset(2020, 1, 6, 00, 00, 00, 000, TimeSpan.Zero);

      var inputDateStartOfDay = inputDate.ToStartOfDay();

      Assert.Equal(
        comparison,
        inputDateStartOfDay.CompareTo(startOfDayOn2020106)
      );
    }

    [Theory]
    [InlineData(new[] {2019, 12, 5, 23, 59, 59, 999, 9998}, -1)]
    [InlineData(new[] {2020, 1, 5, 23, 59, 59, 999, 9998}, 0)]
    [InlineData(new[] {2020, 1, 5, 23, 59, 59, 999, 9999}, 0)]
    [InlineData(new[] {2020, 1, 6, 0, 0, 0, 0, 0}, 0)]
    [InlineData(new[] {2020, 1, 6, 0, 0, 0, 0, 1}, 0)]
    [InlineData(new[] {2020, 1, 6, 12, 0, 0, 0, 0}, 0)]
    [InlineData(new[] {2020, 1, 6, 23, 59, 59, 999, 9998}, 0)]
    [InlineData(new[] {2020, 1, 6, 23, 59, 59, 999, 9999}, 0)]
    [InlineData(new[] {2020, 1, 7, 0, 0, 0, 0, 0}, 0)]
    [InlineData(new[] {2020, 1, 7, 0, 0, 0, 0, 1}, 0)]
    [InlineData(new[] {2020, 2, 7, 0, 0, 0, 0, 0}, 1)]
    [InlineData(new[] {2020, 2, 7, 0, 0, 0, 0, 1}, 1)]
    public void StartOfMonthValuesCompareCorrectly(
      int[] dateArguments,
      int comparison
    ) {
      var inputDate = BuildDateTimeOffset(dateArguments);

      var startOfDayOn2020101 =
        new DateTimeOffset(2020, 01, 01, 00, 00, 00, 000, TimeSpan.Zero);

      var inputDateStartOfDay = inputDate.ToStartOfMonth();

      Assert.Equal(
        comparison,
        inputDateStartOfDay.CompareTo(startOfDayOn2020101)
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
  }
}
