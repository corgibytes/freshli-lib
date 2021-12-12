using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using Xunit;
using VerifyTests;
using VerifyXunit;

namespace Corgibytes.Freshli.Lib.Test.Integration
{
    [UsesVerify]
    public class ManifestFinderRegistryLoaderTest
    {
        [Fact]
        public Task Load()
        {
            var loggerFactory = new LoggerFactory();
            loggerFactory.AddProvider(LoggerRecording.Start(LogLevel.Debug));

            var loader = new ManifestFinderRegistryLoader(loggerFactory.CreateLogger<ManifestFinderRegistryLoader>());

            var registry = new ManifestFinderRegistry(loggerFactory.CreateLogger<ManifestFinderRegistry>());
            loader.RegisterAll(registry);

            return Verifier.Verify(registry.Finders.Select(f => f.GetType().FullName));
        }
    }
}
