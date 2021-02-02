using System.IO;
using System.Linq;

namespace Freshli.Languages.CSharp {
  public class NuGetManifestFinder : IManifestFinder {
    private string _manifestPattern = "*.csproj";

    public IFileHistoryFinder FileFinder { get; set; }

    public IPackageRepository RepositoryFor(string projectRootPath) {
      return new NuGetRepository();
    }

    public IManifest ManifestFor(string projectRootPath) {
      return new NuGetManifest();
    }

    public string[] GetManifestFilenames(string projectRootPath) {
      return FileFinder.GetManifestFilenames(projectRootPath, _manifestPattern);
    }
  }
}
