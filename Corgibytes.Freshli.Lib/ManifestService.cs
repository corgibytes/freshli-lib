using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using Microsoft.Extensions.Logging;

namespace Corgibytes.Freshli.Lib{

    public class ManifestService: IManifestService
    {
        public ILogger<ManifestService> Logger { get; }
        private readonly string _projectRootPath;

        private static readonly IList<AbstractManifestFinder> _finders =
            new List<AbstractManifestFinder>();

        public static IList<AbstractManifestFinder> Finders => _finders;

        public AbstractManifestFinder Finder { get; }

        public string[] ManifestFiles =>
            Finder.GetManifestFilenames(_projectRootPath);

        public bool Successful { get; }

        public LibYearCalculator Calculator => new LibYearCalculator(
            Finder.RepositoryFor(_projectRootPath),
            Finder.ManifestFor(_projectRootPath)
        );

        public ManifestFinder(
            string projectRootPath,
            IFileHistoryFinder fileFinder,
            ILogger<ManifestFinder> logger
        )
        {
            Logger = logger;
            _projectRootPath = projectRootPath;
            Successful = false;
            // TODO: Move this logic out of the constructor. This can cause problems
            foreach (var finder in Finders.ToImmutableList())
            {
                finder.FileFinder = fileFinder;
                if (finder.GetManifestFilenames(projectRootPath).Any())
                {
                    Finder = finder;
                    Successful = true;
                    break;
                }
            }
        }

        public static void Register(AbstractManifestFinder finder)
        {
            Finders.Add(finder);
        }

        // TODO: Move move finder registration to a different project
    }
}
