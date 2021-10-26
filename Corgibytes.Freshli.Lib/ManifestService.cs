using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using NLog;

namespace Corgibytes.Freshli.Lib
{
    public class ManifestService
    {
        // TODO: convert to returning an enumerable of IManifestFinder
        // TODO: It should be possible to get rid of the analysis path parameter, since the IFileHistoryFinder should already know the analysis path
        public IEnumerable<AbstractManifestFinder> SelectFindersFor(string analysisPath, IFileHistoryFinder fileHistoryFinder)
        {
            // TODO: inject the dependency on ManfestFinderRegistry
            foreach (var finder in ManifestFinderRegistry.Finders.ToImmutableList())
            {
                // TODO: the fileHistoryFinder should be passed in to GetManifestFilenames instead of the "analysisPath"
                finder.FileFinder = fileHistoryFinder;
                if (finder.GetManifestFilenames(analysisPath).Any())
                {
                    yield return finder;
                }
            }
        }

    }
}
