using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Corgibytes.Freshli.Lib
{
    public class ManifestService : IManifestService
    {
        private ManifestFinderRegistry _registry;

        public ManifestService(ManifestFinderRegistry registry)
        {
            _registry = registry;
        }

        // TODO: It should be possible to get rid of the analysis path parameter, since the IFileHistoryFinder should already know the analysis path
        public IEnumerable<IManifestFinder> SelectFindersFor(string analysisPath, IFileHistoryFinder fileHistoryFinder)
        {
            // TODO: inject the dependency on ManfestFinderRegistry
            foreach (var finder in _registry.Finders.ToImmutableList())
            {
                // TODO: the fileHistoryFinder should be passed in to GetManifestFilenames instead of the "analysisPath"
                finder.FileFinder = fileHistoryFinder;
                // TODO: I'm not sure this check here makes the most, seems like something the consumer should be responsible
                if (finder.GetManifestFilenames(analysisPath).Any())
                {
                    yield return finder;

                    // TODO: Remove this break to add support for multiple manifests from different providers
                    yield break;
                }
            }
        }

    }
}
