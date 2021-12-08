using System;
using System.Collections.Generic;

namespace Corgibytes.Freshli.Lib
{
    public interface IManifestFinder
    {
        IEnumerable<IManifestHistory> GetManifests(IFileHistorySource fileHistorySource);
    }
}
