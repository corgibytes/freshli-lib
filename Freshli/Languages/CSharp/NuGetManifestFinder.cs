using System.IO;
using System.Linq;

namespace Freshli.Languages.CSharp {
  public class NuGetManifestFinder : IManifestFinder {
    public IFileHistoryFinder FileFinder { get; set; }

    // We have multiple "lock files"
    public string LockFileName => "";

    public bool DoesPathContainManifest(string projectRootPath) {
      return FileFinder.Exists(projectRootPath, "*.csproj");
    }

    public IPackageRepository RepositoryFor(string projectRootPath) {
      return new NuGetRepository();
    }

    public IManifest ManifestFor(string projectRootPath) {
      return new NuGetManifest();
    }
  }
}
