using System.IO;

namespace LibMetrics.Languages.Php
{
  public class PhpComposerManifestFinder: IManifestFinder
  {
    public bool DoesPathContainManifest(string projectRootPath)
    {
      return File.Exists(Path.Combine(projectRootPath, LockFileName));
    }

    public string LockFileName => "composer.lock";

    public IPackageRepository RepositoryFor(string projectRootPath)
    {
      return new MulticastComposerRepository(projectRootPath);
    }

    public IManifest ManifestFor(string projectRootPath)
    {
      return new ComposerManifest();
    }
  }
}
