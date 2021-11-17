using System.Collections.Generic;

namespace Corgibytes.Freshli.Lib
{
    public interface IManifestFinder
    {
        IManifestParser ManifestParser();
        IPackageRepository RepositoryFor(string projectRootPath);
        IEnumerable<string> GetManifestFilenames(string projectRootPath);
    }
}
