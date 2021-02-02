namespace Freshli.Languages.Perl {
  public class CpanfileManifestFinder : IManifestFinder {
    private string _manifestPattern = "cpanfile";

    public IFileHistoryFinder FileFinder { get; set; }

    public IPackageRepository RepositoryFor(string projectRootPath) {
      return new MetaCpanRepository();
    }

    public IManifest ManifestFor(string projectRootPath) {
      return new CpanfileManifest();
    }

    public string[] GetManifestFilenames(string projectRootPath) {
      return FileFinder.GetManifestFilenames(projectRootPath, _manifestPattern);
    }
  }
}
