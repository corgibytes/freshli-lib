namespace LibMetrics.Languages.Ruby
{
  public class RubyBundlerManifestFinder: IManifestFinder
  {
    public IFileHistoryFinder FileFinder { get; set; }

    public bool DoesPathContainManifest(string projectRootPath)
    {
      return FileFinder.Exists(projectRootPath, LockFileName);
    }

    public string LockFileName => "Gemfile.lock";
    public IPackageRepository RepositoryFor(string projectRootPath)
    {
      return new RubyGemsRepository();
    }

    public IManifest ManifestFor(string projectRootPath)
    {
      return new BundlerManifest();
    }
  }
}
