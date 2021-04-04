using System;

namespace Corgibytes.Freshli.Lib.Test
{
  public static class DateBuilder
  {
    public static DateTimeOffset BuildDateTimeOffsetFromParts(int[] dateParts) {
      return new(
        dateParts[0],
        dateParts[1],
        dateParts[2],
        dateParts[3],
        dateParts[4],
        dateParts[5],
        TimeSpan.Zero
      );
    }
  }
}
