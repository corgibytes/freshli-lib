namespace LibMetrics
{
  public interface IManifestFinder
  {
    bool DoesPathContainManifest(string projectRootPath);
    string LockFileName { get; }

    IPackageRepository RepositoryFor(string projectRootPath);
    IManifest ManifestFor(string projectRootPath);

  }
}
