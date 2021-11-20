namespace Corgibytes.Freshli.Lib.Languages.Php
{
    public class PhpComposerManifestFinder : AbstractManifestFinder
    {
        protected override string ManifestPattern => "composer.lock";

        public override IPackageRepository RepositoryFor(string projectRootPath)
        {
            return new MulticastComposerRepository(projectRootPath, FileFinder);
        }

        public override IManifestParser ManifestParser() => new ComposerManifestParser();
    }
}
