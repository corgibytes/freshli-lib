using System;
using System.Collections.Generic;
using System.IO;

namespace Corgibytes.Freshli.Lib
{
    public interface IManifestParser
    {
        IEnumerable<PackageInfo> Parse(Stream contentsStream);
        bool UsesExactMatches { get; }
    }
}
