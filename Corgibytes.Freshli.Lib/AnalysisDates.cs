using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Corgibytes.Freshli.Lib.Util;

namespace Corgibytes.Freshli.Lib {
  public class AnalysisDates : IEnumerable<DateTime> {
    private List<DateTime> _dates = new List<DateTime>();

    public AnalysisDates(IFileHistory history, DateTime asOf) {
      if (history.Dates.Count == 0) {
        _dates.Add(asOf);
        return;
      }

      if (history.Dates.Count == 1 && asOf <= history.Dates[0]) {
        _dates.Add(asOf);
        return;
      }

      var date = history.Dates.First();

      if (date.Day > 1) {
        date = date.AddDays(-date.Day + 1).Date;
        date = date.AddMonths(1).Date;
      }

      while (date <= asOf) {
        var dayOf = date.ToEndOfDay();
        _dates.Add(dayOf);
        date = date.AddMonths(1);
      }

      // include today's date:
      _dates.Add(DateTime.Today);
    }

    public IEnumerator<DateTime> GetEnumerator() {
      return _dates.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() {
      return GetEnumerator();
    }
  }
}
