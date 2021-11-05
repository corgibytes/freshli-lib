using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;

namespace Corgibytes.Freshli.Lib
{
    public class FileHistoryService
    {
        public IFileHistoryFinder SelectFinderFor(string projectRootPath)
        {
            // TODO: Inject dependency on FileHistoryFinderRegistry
            foreach (var finder in FileHistoryFinderRegistry.Finders.ToImmutableList())
            {
                if (finder.DoesPathContainHistorySource(projectRootPath))
                {
                    return finder;
                }
            }

            return null;
        }
    }
}
