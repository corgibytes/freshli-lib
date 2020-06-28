using System;
using System.Collections.Generic;

namespace Freshli {
  public interface IFileHistory {
    IList<DateTime> Dates { get; }
    string ContentsAsOf(DateTime date);
  }
}
