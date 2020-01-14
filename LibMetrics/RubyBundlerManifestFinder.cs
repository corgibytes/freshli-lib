using System.IO;

namespace LibMetrics
{
  public class RubyBundlerManifestFinder: IManifestFinder
  {
    public bool DoesPathContainManifest(string projectRootPath)
    {
      return File.Exists(Path.Combine(projectRootPath, LockFileName));
    }

    public string LockFileName => "Gemfile.lock";
    public IPackageRepository Repository => new RubyGemsRepository();
    public IManifest Manifest => new BundlerManifest();
  }
}
