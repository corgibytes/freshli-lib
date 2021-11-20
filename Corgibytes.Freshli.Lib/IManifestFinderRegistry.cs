using System.Collections.Generic;

namespace Corgibytes.Freshli.Lib
{
    public interface IManifestFinderRegistry
    {
        IList<AbstractManifestFinder> Finders { get; }

        void Register(AbstractManifestFinder finder);
    }
}
