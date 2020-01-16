using System;
using System.Collections.Generic;

namespace LibMetrics
{
  public interface IFileHistory
  {
    IList<DateTime> Dates { get; }
    string ContentsAsOf(DateTime date);
  }
}
