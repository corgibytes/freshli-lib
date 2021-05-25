using System.IO;
using System.Linq;

namespace Corgibytes.Freshli.Lib.Languages.CSharp
{
    public class NuGetManifestFinder : AbstractManifestFinder
    {
        protected override string ManifestPattern => "*.csproj";

        public override IPackageRepository RepositoryFor(string projectRootPath)
        {
            return new NuGetRepository();
        }

        public override IManifest ManifestFor(string projectRootPath)
        {
            return new NuGetManifest();
        }
    }
}
