namespace Freshli
{
  public interface IManifestFinder
  {
    IFileHistoryFinder FileFinder { get; set; }

    bool DoesPathContainManifest(string projectRootPath);
    string LockFileName { get; }

    IPackageRepository RepositoryFor(string projectRootPath);
    IManifest ManifestFor(string projectRootPath);

  }
}
