using System.IO;
using System.Linq;

namespace Freshli.Languages.CSharp {
  public class NuGetManifestFinder : IManifestFinder {
    public IFileHistoryFinder FileFinder { get; set; }

    // We have multiple "lock files" that we'll need to address,
    // but for now just using wildcard .csproj
    public string LockFileName => "*.csproj";

    public bool DoesPathContainManifest(string projectRootPath) {
      return FileFinder.Exists(projectRootPath, LockFileName);
    }

    public IPackageRepository RepositoryFor(string projectRootPath) {
      return new NuGetRepository();
    }

    public IManifest ManifestFor(string projectRootPath) {
      return new NuGetManifest();
    }
  }
}
