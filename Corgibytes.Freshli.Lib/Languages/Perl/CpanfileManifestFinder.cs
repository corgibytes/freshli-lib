namespace Corgibytes.Freshli.Lib.Languages.Perl
{
    public class CpanfileManifestFinder : AbstractManifestFinder
    {
        protected override string ManifestPattern => "cpanfile";

        public override IPackageRepository RepositoryFor(string projectRootPath)
        {
            return new MetaCpanRepository();
        }

        public override IManifest ManifestFor(string projectRootPath)
        {
            return new CpanfileManifest();
        }

        public override IManifestParser ManifestParser() => new CpanfileManifestParser();
    }
}
