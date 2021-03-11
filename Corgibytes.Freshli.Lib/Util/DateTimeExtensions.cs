using System;

namespace Corgibytes.Freshli.Lib.Util {
  public static class DateTimeExtensions {
    public static DateTime ToEndOfDay(this DateTime date) {
      return date.Date.AddDays(1).AddTicks(-1);
    }
  }
}
