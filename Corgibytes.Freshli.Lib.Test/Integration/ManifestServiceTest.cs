using System.Linq;
using Xunit;

namespace Corgibytes.Freshli.Lib.Test.Integration
{
    public class ManifestFinderTest
    {
        public ManifestFinderTest()
        {
            ManifestFinderRegistry.RegisterAll();
        }

        [Fact]
        public void Empty()
        {
            var emptyFixturePath = Fixtures.Path("empty");
            var fileFinder = new FileHistoryFinder(emptyFixturePath);
            var manifestService = new ManifestService();
            var finders = manifestService.SelectFindersFor(emptyFixturePath, fileFinder.Finder);

            Assert.Empty(finders);
        }

        [Fact]
        public void RubyBundler()
        {
            var rubyFixturePath = Fixtures.Path("ruby", "nokotest");

            var fileFinder = new FileHistoryFinder(rubyFixturePath);
            var manifestService = new ManifestService();
            var finders = manifestService.SelectFindersFor(rubyFixturePath, fileFinder.Finder);

            Assert.Equal("Gemfile.lock", finders.First().GetManifestFilenames(rubyFixturePath).First());
        }

        [Fact]
        public void PhpComposer()
        {
            var phpFixturePath = Fixtures.Path("php", "small");

            var fileFinder = new FileHistoryFinder(phpFixturePath);
            var manifestService = new ManifestService();
            var finders = manifestService.SelectFindersFor(phpFixturePath, fileFinder.Finder);

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

            var fileFinder = new FileHistoryFinder(pythonFixturePath);
            var manifestService = new ManifestService();
            var finders = manifestService.SelectFindersFor(pythonFixturePath, fileFinder.Finder);

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

            var fileFinder = new FileHistoryFinder(fixturePath);
            var manifestService = new ManifestService();
            var finders = manifestService.SelectFindersFor(fixturePath, fileFinder.Finder);

            Assert.Equal("cpanfile", finders.First().GetManifestFilenames(fixturePath).First());
        }
    }
}
