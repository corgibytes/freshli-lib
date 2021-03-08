using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Freshli.Util;

namespace Freshli {
  public class AnalysisDates : IEnumerable<DateTimeOffset> {
    private List<DateTimeOffset> _dates = new();

    public AnalysisDates(IFileHistory history, DateTimeOffset asOf) {
      if (history.Dates.Count == 0) {
        _dates.Add(asOf);
      } else if (history.Dates.Count == 1 && asOf <= history.Dates[0]) {
        _dates.Add(asOf);
      } else {
        var date = history.Dates.First();

        if (date.Day > 1) {
          date = date.AddDays(-date.Day + 1).ToStartOfDay();
          date = date.AddMonths(1);
        }

        while (date <= asOf) {
          var dayOf = date.ToStartOfDay();
          _dates.Add(dayOf);
          date = date.AddMonths(1);
        }
      }
    }

    public IEnumerator<DateTimeOffset> GetEnumerator() {
      return _dates.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() {
      return GetEnumerator();
    }
  }
}
