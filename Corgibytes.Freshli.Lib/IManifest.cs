using System;
using System.Collections.Generic;

namespace Corgibytes.Freshli.Lib
{
    public interface IManifest
    {
        IPackageRepository Repository { get; }
        IEnumerable<PackageInfo> Contents { get; }
        bool UsesExactMatches { get; }
        string Revision { get; }
        string FilePath { get; }

        // TODO: Is there any value in created a backlink to the IManifestHistory that this belongs to?
    }
}
