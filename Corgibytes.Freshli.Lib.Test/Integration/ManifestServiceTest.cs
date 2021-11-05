using System.Linq;
using Xunit;

namespace Corgibytes.Freshli.Lib.Test.Integration
{
    public class ManifestFinderTest
    {
        public IFileHistoryFinderRegistry FileHistoryFinderRegistry { get; init; }

        public ManifestFinderTest()
        {
            ManifestFinderRegistry.RegisterAll();

            FileHistoryFinderRegistry = new FileHistoryFinderRegistry();
            FileHistoryFinderRegistry.Register<LocalFileHistoryFinder>();
            FileHistoryFinderRegistry.Register<GitFileHistoryFinder>();
        }

        [Fact]
        public void Empty()
        {
            var emptyFixturePath = Fixtures.Path("empty");
            var historyService = new FileHistoryService(FileHistoryFinderRegistry);
            var fileFinder = historyService.SelectFinderFor(emptyFixturePath);
            var manifestService = new ManifestService();
            var finders = manifestService.SelectFindersFor(emptyFixturePath, fileFinder);

            Assert.Empty(finders);
        }

        [Fact]
        public void RubyBundler()
        {
            var rubyFixturePath = Fixtures.Path("ruby", "nokotest");

            var historyService = new FileHistoryService(FileHistoryFinderRegistry);
            var fileFinder = historyService.SelectFinderFor(rubyFixturePath);
            var manifestService = new ManifestService();
            var finders = manifestService.SelectFindersFor(rubyFixturePath, fileFinder);

            Assert.Equal("Gemfile.lock", finders.First().GetManifestFilenames(rubyFixturePath).First());
        }

        [Fact]
        public void PhpComposer()
        {
            var phpFixturePath = Fixtures.Path("php", "small");

            var historyService = new FileHistoryService(FileHistoryFinderRegistry);
            var fileFinder = historyService.SelectFinderFor(phpFixturePath);
            var manifestService = new ManifestService();
            var finders = manifestService.SelectFindersFor(phpFixturePath, fileFinder);

            Assert.Equal("composer.lock", finders.First().GetManifestFilenames(phpFixturePath).First());
        }

        [Fact]
        public void PythonPipRequirementsTxt()
        {
            var pythonFixturePath = Fixtures.Path(
              "python",
              "requirements-txt",
              "small"
            );

            var historyService = new FileHistoryService(FileHistoryFinderRegistry);
            var fileFinder = historyService.SelectFinderFor(pythonFixturePath);
            var manifestService = new ManifestService();
            var finders = manifestService.SelectFindersFor(pythonFixturePath, fileFinder);

            Assert.Equal("requirements.txt", finders.First().GetManifestFilenames(pythonFixturePath).First());
        }


        [Fact]
        public void PerlCpanfile()
        {
            var fixturePath = Fixtures.Path(
              "perl",
              "cpanfile",
              "simple-without-snapshot"
            );

            var historyService = new FileHistoryService(FileHistoryFinderRegistry);
            var fileFinder = historyService.SelectFinderFor(fixturePath);
            var manifestService = new ManifestService();
            var finders = manifestService.SelectFindersFor(fixturePath, fileFinder);

            Assert.Equal("cpanfile", finders.First().GetManifestFilenames(fixturePath).First());
        }
    }
}
