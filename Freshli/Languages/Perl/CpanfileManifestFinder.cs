namespace Freshli.Languages.Perl {
  public class CpanfileManifestFinder: IManifestFinder {
    public IFileHistoryFinder FileFinder { get; set; }
    public bool DoesPathContainManifest(string projectRootPath) {
      return FileFinder.Exists(projectRootPath, LockFileName);
    }

    public string LockFileName => "cpanfile";
    public IPackageRepository RepositoryFor(string projectRootPath) {
      return new MetaCpanRepository();
    }

    public IManifest ManifestFor(string projectRootPath) {
      return new CpanfileManifest();
    }
  }
}
