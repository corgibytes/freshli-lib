namespace Freshli.Languages.Python {
  public class PipRequirementsTxtManifestFinder : IManifestFinder {
    private string _manifestPattern = "requirements.txt";
    
    public IFileHistoryFinder FileFinder { get; set; }

    public IPackageRepository RepositoryFor(string projectRootPath) {
      return new PyPIRepository();
    }

    public IManifest ManifestFor(string projectRootPath) {
      return new PipRequirementsTxtManifest();
    }

    public string[] GetManifestFilenames(string projectRootPath) {
      return FileFinder.GetManifestFilenames(projectRootPath, _manifestPattern);
    }
  }
}
