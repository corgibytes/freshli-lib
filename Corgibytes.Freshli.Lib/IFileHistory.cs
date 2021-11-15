using System;
using System.Collections.Generic;

namespace Corgibytes.Freshli.Lib
{
    public interface IFileHistory
    {
        IEnumerable<DateTimeOffset> Dates { get; }
        string ContentsAsOf(DateTimeOffset date);
        string ShaAsOf(DateTimeOffset date);
    }
}
