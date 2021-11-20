using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using Corgibytes.Freshli.Lib.Exceptions;

namespace Corgibytes.Freshli.Lib
{
    public class FileHistoryService : IFileHistoryService
    {
        private IFileHistoryFinderRegistry _registry;

        public FileHistoryService(IFileHistoryFinderRegistry registry)
        {
            _registry = registry;
        }
        public IFileHistoryFinder SelectFinderFor(string projectRootPath)
        {
            foreach (var finder in _registry.Finders.ToImmutableList())
            {
                if (finder.DoesPathContainHistorySource(projectRootPath))
                {
                    return finder;
                }
            }

            throw new FileHistoryFinderNotFoundException(projectRootPath);
        }
    }
}
