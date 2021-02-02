namespace Freshli.Languages.Php {
  public class PhpComposerManifestFinder : IManifestFinder {
    private readonly string _manifestPattern = "composer.lock";
    public IFileHistoryFinder FileFinder { get; set; }

    public IPackageRepository RepositoryFor(string projectRootPath) {
      return new MulticastComposerRepository(projectRootPath, FileFinder);
    }

    public IManifest ManifestFor(string projectRootPath) {
      return new ComposerManifest();
    }

    public string[] GetManifestFilenames(string projectRootPath) {
      return FileFinder.GetManifestFilenames(projectRootPath, _manifestPattern);
    }
  }
}
