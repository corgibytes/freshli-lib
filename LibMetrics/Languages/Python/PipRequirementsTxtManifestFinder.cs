namespace LibMetrics.Languages.Python {
  public class PipRequirementsTxtManifestFinder: IManifestFinder {
    public IFileHistoryFinder FileFinder { get; set; }
    public bool DoesPathContainManifest(string projectRootPath) {
      throw new System.NotImplementedException();
    }

    public string LockFileName { get; }
    public IPackageRepository RepositoryFor(string projectRootPath) {
      throw new System.NotImplementedException();
    }

    public IManifest ManifestFor(string projectRootPath) {
      throw new System.NotImplementedException();
    }
  }
}