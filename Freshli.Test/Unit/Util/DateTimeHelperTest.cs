using System;
using Freshli.Util;
using Xunit;

namespace Freshli.Test.Unit.Util {
  public class DateTimeHelperTest {
    private readonly DateTime _baseDateTime =
      new DateTime(2020, 1, 6, 23, 59, 59, 999).AddTicks(9999);

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
    public void NumericValuesAreComparedCorrectly(
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
      var inputDate = new DateTime(
        year,
        month,
        day,
        hour,
        minute,
        second,
        millisecond
      ).AddTicks(tick);

      var inputDateEoD = DateTimeHelper.ConvertToEndOfDay(inputDate);

      Assert.Equal(
        comparison,
        inputDateEoD.CompareTo(_baseDateTime)
      );
    }
  }
}
