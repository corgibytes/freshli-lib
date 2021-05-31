namespace Corgibytes.Freshli.Lib
{
    public abstract class AbstractManifestFinder
    {
        protected abstract string ManifestPattern { get; }

        public IFileHistoryFinder FileFinder { get; set; }

        public abstract IPackageRepository RepositoryFor(string projectRootPath);
        public abstract IManifest ManifestFor(string projectRootPath);

        public string[] GetManifestFilenames(string projectRootPath)
        {
            return FileFinder.GetManifestFilenames(projectRootPath, ManifestPattern);
        }
    }
}
