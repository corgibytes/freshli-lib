namespace Corgibytes.Freshli.Lib.Languages.Python
{
    public class PipRequirementsTxtManifestFinder : AbstractManifestFinder
    {
        protected override string ManifestPattern => "requirements.txt";

        public override IPackageRepository RepositoryFor(string projectRootPath)
        {
            return new PyPIRepository();
        }

        public override IManifestParser ManifestParser() => new PipRequirementsTxtManifestParser();
    }
}
