namespace Freshli.Languages.Ruby {
  public class RubyBundlerManifestFinder : IManifestFinder {
    private string _manifestPattern = "Gemfile.lock";

    public IFileHistoryFinder FileFinder { get; set; }

    public IPackageRepository RepositoryFor(string projectRootPath) {
      return new RubyGemsRepository();
    }

    public IManifest ManifestFor(string projectRootPath) {
      return new BundlerManifest();
    }

    public string[] GetManifestFilenames(string projectRootPath) {
      return FileFinder.GetManifestFilenames(projectRootPath, _manifestPattern);
    }
  }
}
