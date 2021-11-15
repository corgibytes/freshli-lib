using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NLog;

namespace Corgibytes.Freshli.Lib.Languages
{
    // TODO: Move in the direction of `IManifest` instances being mostly immutable, and have them be instantiated by calling an `IManifestParser.Parse` method. This is going to help with `async`/`await` implementation.
    public abstract class AbstractManifest : IManifest
    {
        protected static readonly Logger _logger = LogManager.
          GetCurrentClassLogger();

        private IDictionary<string, PackageInfo> _packages =
          new Dictionary<string, PackageInfo>();

        public int Count => _packages.Count;
        public virtual bool UsesExactMatches => Parser.UsesExactMatches;

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
            _logger.Trace(
              $"AddPackage: PackageInfo({packageName}, {packageVersion})"
            );
        }

        public void Clear()
        {
            _packages.Clear();
        }

        protected virtual IManifestParser Parser { get; }

        // TODO: Remove this method after all IManifestParser classes and implemented
        public virtual void Parse(string contents)
        {
            Clear();
            MemoryStream stream = new MemoryStream(UTF8Encoding.UTF8.GetBytes(contents));
            foreach (var package in Parser.Parse(stream))
            {
                this.Add(package.Name, package.Version);
            }
        }

        public PackageInfo this[string packageName] => _packages[packageName];
    }
}
