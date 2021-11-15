using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Corgibytes.Freshli.Lib.Util;

namespace Corgibytes.Freshli.Lib
{
    public class AnalysisDates : IEnumerable<DateTimeOffset>
    {
        IFileHistory history;
        DateTimeOffset asOf;

        public AnalysisDates(IFileHistory history, DateTimeOffset asOf)
        {
            this.history = history;
            this.asOf = asOf;
        }

        public IEnumerator<DateTimeOffset> GetEnumerator()
        {
            var dateCount = history.Dates.Count();

            if (dateCount == 0)
            {
                yield return asOf;
            }
            else if (dateCount == 1 && asOf <= history.Dates.First())
            {
                yield return asOf;
            }
            else
            {
                var date = history.Dates.First().ToOffset(asOf.Offset);

                if (date != date.ToStartOfMonth())
                {
                    yield return date;
                    date = date.AddMonths(1).ToStartOfMonth();
                }

                while (date <= asOf)
                {
                    yield return date;
                    date = date.AddMonths(1);
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
