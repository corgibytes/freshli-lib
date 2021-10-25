using System;
using System.Collections.Generic;

namespace Corgibytes.Freshli.Lib
{
    public interface IManifestService
    {
        IEnumerable<IManifestFinder> SelectFindersFor(string analysisPath, IFileHistoryFinder fileHistoryFinder);
    }
}
