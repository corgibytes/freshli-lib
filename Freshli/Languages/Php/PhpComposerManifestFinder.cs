namespace Freshli.Languages.Php {
  public class PhpComposerManifestFinder : IManifestFinder {
    public IFileHistoryFinder FileFinder { get; set; }

    public bool DoesPathContainManifest(string projectRootPath) {
      return FileFinder.Exists(projectRootPath, LockFileName);
    }

    public string LockFileName => "composer.lock";

    public IPackageRepository RepositoryFor(string projectRootPath) {
      return new MulticastComposerRepository(projectRootPath, FileFinder);
    }

    public IManifest ManifestFor(string projectRootPath) {
      return new ComposerManifest();
    }
  }
}
