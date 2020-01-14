namespace LibMetrics
{
  public interface IManifestFinder
  {
    bool DoesPathContainManifest(string projectRootPath);
    string LockFileName { get; }

    IPackageRepository Repository { get; }
    IManifest Manifest { get; }

  }
}
