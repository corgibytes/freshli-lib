using System.Collections.Generic;

namespace Corgibytes.Freshli.Lib
{
    public abstract class ToRemoveAbstractManifestFinder
    {
        public abstract string SearchPattern { get; }
        public abstract IPackageRepository RepositoryFor(IFileHistory fileHistory, string projectRootPath);
        public abstract IManifestParser ManifestParser();
    }
}
