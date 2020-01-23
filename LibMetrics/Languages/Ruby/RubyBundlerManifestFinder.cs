using System.IO;

namespace LibMetrics.Languages.Ruby
{
  public class RubyBundlerManifestFinder: IManifestFinder
  {
    public bool DoesPathContainManifest(string projectRootPath)
    {
      return File.Exists(Path.Combine(projectRootPath, LockFileName));
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
