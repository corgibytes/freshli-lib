using Polly.Registry;

using Microsoft.Extensions.Logging;

namespace Corgibytes.Freshli.Lib.Languages.Ruby
{
    public class RubyBundlerManifestFinder : AbstractManifestFinder
    {
        private IReadOnlyPolicyRegistry<string> _policyRegistry;

        private ILogger<IPackageRepository> _logger;

        public RubyBundlerManifestFinder(IReadOnlyPolicyRegistry<string> policyRegistry, ILogger<IPackageRepository> logger)
        {
            _policyRegistry = policyRegistry;
            _logger = logger;
        }
        protected override string ManifestPattern => "Gemfile.lock";

        public override IPackageRepository RepositoryFor(string projectRootPath)
        {
            // TODO: Explore ways to make this injectable
            // Perhaps what could be injected is a `Func<IPackageRepository, string>` that can be
            // called to retrieve the instance.
            return new RubyGemsRepository(_policyRegistry, _logger);
        }

        public override IManifest ManifestFor(string projectRootPath)
        {
            return new BundlerManifest();
        }
    }
}
