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
        // TODO: inject this dependency
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly string _projectRootPath;

        public AbstractManifestFinder Finder { get; }

        public string[] ManifestFiles =>
            Finder.GetManifestFilenames(_projectRootPath);

        public bool Successful { get; }

        public LibYearCalculator Calculator => new LibYearCalculator(
            Finder.RepositoryFor(_projectRootPath),
            Finder.ManifestFor(_projectRootPath)
        );

        // TODO: rework this logic so that it does not run as part of the constructor
        public ManifestService(
            string projectRootPath,
            IFileHistoryFinder fileFinder
        )
        {
            _projectRootPath = projectRootPath;
            Successful = false;
            // TODO: inject the dependency on ManfestFinderRegistry
            foreach (var finder in ManifestFinderRegistry.Finders.ToImmutableList())
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

    }
}
