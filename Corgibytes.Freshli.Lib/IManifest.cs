using System.Collections.Generic;

namespace Corgibytes.Freshli.Lib
{
    public interface IManifest : IEnumerable<PackageInfo>
    {
        int Count { get; }
        void Add(string packageName, string packageVersion);
        // TODO: Create an async version of this method.
        void Parse(string contents);
        PackageInfo this[string packageName] { get; }
        bool UsesExactMatches { get; }
    }
}
