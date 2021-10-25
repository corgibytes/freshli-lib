namespace Corgibytes.Freshli.Lib
{
    // TODO: Extract an interface from this class. Update all references to use the interface
    public abstract class AbstractManifestFinder: IManifestFinder
    {
        protected abstract string ManifestPattern { get; }

        public IFileHistoryFinder FileFinder { get; set; }

        // TODO: Create an async version of this method
        public abstract IPackageRepository RepositoryFor(string projectRootPath);
        // TODO: Create an async version of this method
        public abstract IManifest ManifestFor(string projectRootPath);

        // TODO: Create an async version of this method
        public string[] GetManifestFilenames(string projectRootPath)
        {
            return FileFinder.GetManifestFilenames(projectRootPath, ManifestPattern);
        }
    }
}
