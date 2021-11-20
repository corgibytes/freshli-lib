namespace Corgibytes.Freshli.Lib.Languages.Ruby
{
    public class RubyBundlerManifestFinder : AbstractManifestFinder
    {
        protected override string ManifestPattern => "Gemfile.lock";

        public override IPackageRepository RepositoryFor(string projectRootPath)
        {
            return new RubyGemsRepository();
        }

        public override IManifestParser ManifestParser() => new BundlerManifestParser();
    }
}
