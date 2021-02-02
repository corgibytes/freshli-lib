namespace Freshli {
  public interface IManifestFinder {
    IFileHistoryFinder FileFinder { get; set; }

    IPackageRepository RepositoryFor(string projectRootPath);
    IManifest ManifestFor(string projectRootPath);

    string[] GetManifestFilenames(string projectRootPath);
  }
}
