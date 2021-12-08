using System.Collections.Generic;

namespace Corgibytes.Freshli.Lib
{
    public interface IManifestFinderRegistry
    {
        IList<IManifestFinder> Finders { get; }

        void Register(IManifestFinder finder);
    }
}
