using System;
using System.Collections.Generic;

namespace Corgibytes.Freshli.Lib {
  public interface IFileHistory {
    IList<DateTime> Dates { get; }
    string ContentsAsOf(DateTime date);
    string ShaAsOf(DateTime date);
  }
}
