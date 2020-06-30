namespace Freshli.Languages.Python {
  public class PipRequirementsTxtManifestFinder : IManifestFinder {
    public IFileHistoryFinder FileFinder { get; set; }

    public bool DoesPathContainManifest(string projectRootPath) {
      return FileFinder.Exists(projectRootPath, LockFileName);
    }

    public string LockFileName => "requirements.txt";

    public IPackageRepository RepositoryFor(string projectRootPath) {
      return new PyPIRepository();
    }

    public IManifest ManifestFor(string projectRootPath) {
      return new PipRequirementsTxtManifest();
    }
  }
}
