using System;
using Freshli.Util;
using Xunit;

namespace Freshli.Test.Unit.Util {
  public class DateTimeExtensionsTest {
    [Theory]
    [InlineData(2020, 1, 5, 23, 59, 59, 999, 9998, -1)]
    [InlineData(2020, 1, 5, 23, 59, 59, 999, 9999, -1)]
    [InlineData(2020, 1, 6, 0, 0, 0, 0, 0, 0)]
    [InlineData(2020, 1, 6, 0, 0, 0, 0, 1, 0)]
    [InlineData(2020, 1, 6, 12, 0, 0, 0, 0, 0)]
    [InlineData(2020, 1, 6, 23, 59, 59, 999, 9998, 0)]
    [InlineData(2020, 1, 6, 23, 59, 59, 999, 9999, 0)]
    [InlineData(2020, 1, 7, 0, 0, 0, 0, 0, 1)]
    [InlineData(2020, 1, 7, 0, 0, 0, 0, 1, 1)]
    public void EndOfDayValuesCompareCorrectly(
      int year,
      int month,
      int day,
      int hour,
      int minute,
      int second,
      int millisecond,
      int tick,
      int comparison
    ) {
      var inputDate = new DateTimeOffset(
        year,
        month,
        day,
        hour,
        minute,
        second,
        millisecond,
        TimeSpan.Zero
      ).AddTicks(tick);

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
    [InlineData(2020, 1, 5, 23, 59, 59, 999, 9998, -1)]
    [InlineData(2020, 1, 5, 23, 59, 59, 999, 9999, -1)]
    [InlineData(2020, 1, 6, 0, 0, 0, 0, 0, 0)]
    [InlineData(2020, 1, 6, 0, 0, 0, 0, 1, 0)]
    [InlineData(2020, 1, 6, 12, 0, 0, 0, 0, 0)]
    [InlineData(2020, 1, 6, 23, 59, 59, 999, 9998, 0)]
    [InlineData(2020, 1, 6, 23, 59, 59, 999, 9999, 0)]
    [InlineData(2020, 1, 7, 0, 0, 0, 0, 0, 1)]
    [InlineData(2020, 1, 7, 0, 0, 0, 0, 1, 1)]
    public void StartOfDayValuesCompareCorrectly(
      int year,
      int month,
      int day,
      int hour,
      int minute,
      int second,
      int millisecond,
      int tick,
      int comparison
    ) {
      var inputDate = new DateTimeOffset(
        year,
        month,
        day,
        hour,
        minute,
        second,
        millisecond,
        TimeSpan.Zero
      ).AddTicks(tick);

      var startOfDayOn2020106 =
        new DateTimeOffset(2020, 1, 6, 00, 00, 00, 000, TimeSpan.Zero);

      var inputDateStartOfDay = inputDate.ToStartOfDay();

      Assert.Equal(
        comparison,
        inputDateStartOfDay.CompareTo(startOfDayOn2020106)
      );
    }

    [Theory]
    [InlineData(2019, 12, 5, 23, 59, 59, 999, 9998, -1)]
    [InlineData(2020, 1, 5, 23, 59, 59, 999, 9998, 0)]
    [InlineData(2020, 1, 5, 23, 59, 59, 999, 9999, 0)]
    [InlineData(2020, 1, 6, 0, 0, 0, 0, 0, 0)]
    [InlineData(2020, 1, 6, 0, 0, 0, 0, 1, 0)]
    [InlineData(2020, 1, 6, 12, 0, 0, 0, 0, 0)]
    [InlineData(2020, 1, 6, 23, 59, 59, 999, 9998, 0)]
    [InlineData(2020, 1, 6, 23, 59, 59, 999, 9999, 0)]
    [InlineData(2020, 1, 7, 0, 0, 0, 0, 0, 0)]
    [InlineData(2020, 1, 7, 0, 0, 0, 0, 1, 0)]
    [InlineData(2020, 2, 7, 0, 0, 0, 0, 0, 1)]
    [InlineData(2020, 2, 7, 0, 0, 0, 0, 0, 1)]
    public void StartOfMonthValuesCompareCorrectly(
      int year,
      int month,
      int day,
      int hour,
      int minute,
      int second,
      int millisecond,
      int tick,
      int comparison
    ) {
      var inputDate = new DateTimeOffset(
        year,
        month,
        day,
        hour,
        minute,
        second,
        millisecond,
        TimeSpan.Zero
      ).AddTicks(tick);

      var startOfDayOn2020101 =
        new DateTimeOffset(2020, 01, 01, 00, 00, 00, 000, TimeSpan.Zero);

      var inputDateStartOfDay = inputDate.ToStartOfMonth();

      Assert.Equal(
        comparison,
        inputDateStartOfDay.CompareTo(startOfDayOn2020101)
      );
    }

  }
}
