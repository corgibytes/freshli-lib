using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;

namespace Corgibytes.Freshli.Lib
{
    // TODO: Rename this to FileHistoryService
    public class FileHistoryFinder
    {
        private readonly string _projectRootPath;

        private static readonly IList<IFileHistoryFinder> _finders =
          new List<IFileHistoryFinder>();

        public static IList<IFileHistoryFinder> Finders => _finders;
        public IFileHistoryFinder Finder { get; }

        public FileHistoryFinder(string projectRootPath)
        {
            _projectRootPath = projectRootPath;
            Finder = new LocalFileHistoryFinder();

            // Move this logic out of the constructor. Put it in an async method.
            foreach (var finder in Finders.ToImmutableList())
            {
                if (finder.DoesPathContainHistorySource(projectRootPath))
                {
                    Finder = finder;
                    break;
                }
            }
        }

        // TODO: Make an async version of this method
        public IFileHistory FileHistoryOf(string targetFile)
        {
            // Call the method that's been extracted from the constructor from here
            // We it probably makes sense to cache/memoize the result.

            return Finder.FileHistoryOf(_projectRootPath, targetFile);
        }

        public static void Register<TFinder>()
          where TFinder : IFileHistoryFinder, new()
        {
            Finders.Add(new TFinder());
        }
    }
}
