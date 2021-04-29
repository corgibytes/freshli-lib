namespace Corgibytes.Freshli.Lib.Languages.Php
{
    public class PhpComposerManifestFinder : AbstractManifestFinder
    {
        protected override string ManifestPattern => "composer.lock";

        public override IPackageRepository RepositoryFor(string projectRootPath)
        {
            return new MulticastComposerRepository(projectRootPath, FileFinder);
        }

        public override IManifest ManifestFor(string projectRootPath)
        {
            return new ComposerManifest();
        }
    }
}
