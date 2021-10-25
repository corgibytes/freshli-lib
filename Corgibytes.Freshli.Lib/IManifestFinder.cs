using System;
using System.Collections.Generic;

namespace Corgibytes.Freshli.Lib
{
    public interface IManifestFinder
    {
        IEnumerable<string> ManifestFiles { get; }
        bool Successful { get; }
    }
}
