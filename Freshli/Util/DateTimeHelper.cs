using System;

namespace Freshli.Util {
  public static class DateTimeHelper {
    public static DateTime ConvertToEndOfDay(DateTime date) {
      return date.Date.AddDays(1).AddTicks(-1);
    }
  }
}
