using System;

namespace Corgibytes.Freshli.Lib.Test
{
    public static class DateBuilder
    {
        public static DateTimeOffset BuildDateTimeOffsetFromParts(int[] dateParts)
        {
            if (dateParts.Length == 6)
            {
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
            else if (dateParts.Length == 8)
            {
                var inputDate = new DateTimeOffset(
                    dateParts[0],
                    dateParts[1],
                    dateParts[2],
                    dateParts[3],
                    dateParts[4],
                    dateParts[5],
                    dateParts[6],
                    TimeSpan.Zero
                ).AddTicks(dateParts[7]);
                return inputDate;
            }
            else
            {
                throw new ArgumentException(nameof(dateParts), "Wrong number of arguments");
            }
        }
    }
}
