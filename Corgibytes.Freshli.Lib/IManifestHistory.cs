using System;
using System.Collections.Generic;

namespace Corgibytes.Freshli.Lib
{
    public interface IManifestHistory
    {
        IManifest ManifestAsOf(DateTimeOffset asOf);
        string FilePath { get; }
        IEnumerable<DateTimeOffset> Dates { get; }
    }
}
