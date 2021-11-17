using System.Collections.Generic;

namespace Corgibytes.Freshli.Lib
{
    // TODO: Remove this interface
    public interface IManifest : IEnumerable<PackageInfo>
    {
        int Count { get; }
        void Add(string packageName, string packageVersion);
        PackageInfo this[string packageName] { get; }
    }
}
