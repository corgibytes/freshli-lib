using System;
using System.Collections.Generic;
using System.IO;

namespace Corgibytes.Freshli.Lib
{
    public interface IFileHistory
    {
        IEnumerable<DateTimeOffset> Dates { get; }
        string ContentsAsOf(DateTimeOffset date);
        Stream ContentStreamAsOf(DateTimeOffset date);
        string ShaAsOf(DateTimeOffset date);
    }
}
