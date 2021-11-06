using System.Collections.Generic;

namespace Corgibytes.Freshli.Lib
{
    public interface IManifestFinder
    {
        IManifest ManifestFor(string projectRootPath);
        IPackageRepository RepositoryFor(string projectRootPath);
        IEnumerable<string> GetManifestFilenames(string projectRootPath);
    }
}
