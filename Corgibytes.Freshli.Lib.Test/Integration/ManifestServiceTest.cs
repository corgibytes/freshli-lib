using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

using VerifyTests;
using VerifyXunit;
using Xunit;

namespace Corgibytes.Freshli.Lib.Test.Integration
{
    [UsesVerify]
    public class ManifestServiceTest
    {
        public IFileHistoryFinderRegistry FileHistoryFinderRegistry { get; init; }
        public IManifestFinderRegistry ManifestFinderRegistry { get; init; }

        public ILoggerFactory _loggerFactory;

        public ManifestServiceTest()
        {
            _loggerFactory = new LoggerFactory();
            _loggerFactory.AddProvider(LoggerRecording.Start(LogLevel.Trace));

            ManifestFinderRegistry = new ManifestFinderRegistry(_loggerFactory.CreateLogger<ManifestFinderRegistry>());

            var loader = new ManifestFinderRegistryLoader(NLog.LogManager.GetLogger("ManifestFinderRegistryLoader"));
            loader.RegisterAll(ManifestFinderRegistry);

            FileHistoryFinderRegistry = new FileHistoryFinderRegistry();
            FileHistoryFinderRegistry.Register<GitFileHistoryFinder>();
            FileHistoryFinderRegistry.Register<LocalFileHistoryFinder>();
        }

        [Fact]
        public Task Empty()
        {
            var emptyFixturePath = Fixtures.Path("empty");
            var historyService = new FileHistoryService(FileHistoryFinderRegistry);
            var fileFinder = historyService.SelectFinderFor(emptyFixturePath);
            var manifestService = new ManifestService(ManifestFinderRegistry);
            var finders = manifestService.SelectFindersFor(emptyFixturePath, fileFinder);

            return Verifier.Verify(finders.ToImmutableArray().Select<IManifestFinder, string>(f => f.GetType().Name));
        }

        [Fact]
        public Task RubyBundler()
        {
            var rubyFixturePath = Fixtures.Path("ruby", "nokotest");

            var historyService = new FileHistoryService(FileHistoryFinderRegistry);
            var fileFinder = historyService.SelectFinderFor(rubyFixturePath);
            var manifestService = new ManifestService(ManifestFinderRegistry);
            var finders = manifestService.SelectFindersFor(rubyFixturePath, fileFinder);

            return Verifier.Verify(finders.ToImmutableArray().Select<IManifestFinder, string>(f => f.GetType().Name));
        }

        [Fact]
        public Task PhpComposer()
        {
            var phpFixturePath = Fixtures.Path("php", "small");

            var historyService = new FileHistoryService(FileHistoryFinderRegistry);
            var fileFinder = historyService.SelectFinderFor(phpFixturePath);
            var manifestService = new ManifestService(ManifestFinderRegistry);
            var finders = manifestService.SelectFindersFor(phpFixturePath, fileFinder);

            return Verifier.Verify(finders.ToImmutableArray().Select<IManifestFinder, string>(f => f.GetType().Name));
        }

        [Fact]
        public Task PythonPipRequirementsTxt()
        {
            var pythonFixturePath = Fixtures.Path(
              "python",
              "requirements-txt",
              "small"
            );

            var historyService = new FileHistoryService(FileHistoryFinderRegistry);
            var fileFinder = historyService.SelectFinderFor(pythonFixturePath);
            var manifestService = new ManifestService(ManifestFinderRegistry);
            var finders = manifestService.SelectFindersFor(pythonFixturePath, fileFinder);

            return Verifier.Verify(finders.ToImmutableArray().Select<IManifestFinder, string>(f => f.GetType().Name));
        }


        [Fact]
        public Task PerlCpanfile()
        {
            var fixturePath = Fixtures.Path(
              "perl",
              "cpanfile",
              "simple-without-snapshot"
            );

            var historyService = new FileHistoryService(FileHistoryFinderRegistry);
            var fileFinder = historyService.SelectFinderFor(fixturePath);
            var manifestService = new ManifestService(ManifestFinderRegistry);
            var finders = manifestService.SelectFindersFor(fixturePath, fileFinder);

            return Verifier.Verify(finders.ToImmutableArray().Select<IManifestFinder, string>(f => f.GetType().Name));
        }
    }
}
