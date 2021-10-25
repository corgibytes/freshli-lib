using System.Collections;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace Corgibytes.Freshli.Lib.Languages
{
    public abstract class AbstractManifest : IManifest
    {
        private IDictionary<string, PackageInfo> _packages =
            new Dictionary<string, PackageInfo>();

        public ILogger<IManifest> Logger { get; private set; }
        public int Count => _packages.Count;
        public abstract bool UsesExactMatches { get; }

        public AbstractManifest(ILogger<IManifest> logger)
        {
            Logger = logger;
        }

        public IEnumerator<PackageInfo> GetEnumerator()
        {
            return _packages.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(string packageName, string packageVersion)
        {
            _packages[packageName] = new PackageInfo(
                packageName,
                packageVersion
            );
            Logger.LogTrace(
                $"AddPackage: PackageInfo({packageName}, {packageVersion})"
            );
        }

        public void Clear()
        {
            _packages.Clear();
        }

        public abstract void Parse(string contents);
        public PackageInfo this[string packageName] => _packages[packageName];
    }
}
