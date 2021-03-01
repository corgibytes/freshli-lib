using System;
using System.Collections.Generic;

namespace Freshli {
  public interface IFileHistory {
    IList<DateTimeOffset> Dates { get; }
    string ContentsAsOf(DateTimeOffset date);
    string ShaAsOf(DateTimeOffset date);
  }
}
