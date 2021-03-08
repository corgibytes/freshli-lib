using System;

namespace Freshli.Util {
  public static class DateTimeExtensions {
    public static DateTimeOffset ToEndOfDay(this DateTimeOffset date) {
      return date.
        ToStartOfDay().
        AddDays(1).
        AddTicks(-1);
    }

    public static DateTimeOffset ToStartOfDay(this DateTimeOffset date) {
      return date.
        AddHours(-date.Hour).
        AddMinutes(-date.Minute).
        AddSeconds(-date.Second).
        AddMilliseconds(-date.Millisecond).
        AddTicks(-(date.Ticks % 10_000));
    }
  }
}
