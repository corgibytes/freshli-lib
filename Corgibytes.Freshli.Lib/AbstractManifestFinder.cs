namespace Corgibytes.Freshli.Lib
{
    // TODO: extract out IManifestFinder
    public abstract class AbstractManifestFinder
    {
        protected abstract string ManifestPattern { get; }

        public IFileHistoryFinder FileFinder { get; set; }

        public abstract IPackageRepository RepositoryFor(string projectRootPath);
        public abstract IManifest ManifestFor(string projectRootPath);

        // TODO: Have this return a IEnumerable<string> so that it can be implemented in a more async way
        public string[] GetManifestFilenames(string projectRootPath)
        {
            return FileFinder.GetManifestFilenames(projectRootPath, ManifestPattern);
        }
    }
}
