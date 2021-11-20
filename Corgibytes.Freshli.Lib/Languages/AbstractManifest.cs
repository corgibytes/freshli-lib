using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NLog;

namespace Corgibytes.Freshli.Lib.Languages
{
    // TODO: Remove this class
    public abstract class AbstractManifest
    {
        protected static readonly Logger _logger = LogManager.
          GetCurrentClassLogger();

        private IDictionary<string, PackageInfo> _packages =
          new Dictionary<string, PackageInfo>();

        public int Count => _packages.Count;

        public IEnumerator<PackageInfo> GetEnumerator()
        {
            return _packages.Values.GetEnumerator();
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

        public PackageInfo this[string packageName] => _packages[packageName];
    }
}
