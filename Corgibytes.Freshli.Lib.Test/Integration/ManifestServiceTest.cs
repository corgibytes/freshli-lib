using System.Collections.Generic;
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
            FileHistoryFinderRegistry.Register<GitFileHistoryFinder>();
            FileHistoryFinderRegistry.Register<LocalFileHistoryFinder>();
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

        [Fact]
        public void CSharpProjectFile()
        {
            string fixturePath = Fixtures.Path(
                "csharp",
                "csproj"
            );
            AssertManifest(fixturePath, "Project.csproj");
        }

        [Fact]
        public void CSharpPackagesFile()
        {
            string fixturePath = Fixtures.Path(
                "csharp",
                "config"
            );
            AssertManifest(fixturePath, "packages.config");
        }

        [Fact]
        public void CSharpMultipleFiles()
        {
            string fixturePath = Fixtures.Path(
                "csharp"
            );
            var historyService = new FileHistoryService(FileHistoryFinderRegistry);
            var fileFinder = historyService.SelectFinderFor(fixturePath);
            var manifestService = new ManifestService();

            var finders = manifestService.SelectFindersFor(fixturePath, fileFinder);
            List<string> manifestFilenames = finders.SelectMany(finder => finder.GetManifestFilenames(fixturePath)).ToList();

            // When https://github.com/corgibytes/freshli-lib/issues/630 is resolved this assertion
            // should be Assert.Equal(8, manifestFilenames.Count);
            Assert.InRange(manifestFilenames.Count, 2, 8);
            Assert.Contains("Project.csproj", manifestFilenames);
            Assert.Contains("packages.config", manifestFilenames);
        }

        private void AssertManifest(string fixturePath, string expectedManifest)
        {
            var historyService = new FileHistoryService(FileHistoryFinderRegistry);
            var fileFinder = historyService.SelectFinderFor(fixturePath);
            var manifestService = new ManifestService();
            var finders = manifestService.SelectFindersFor(fixturePath, fileFinder);

            Assert.Equal(expectedManifest, finders.First().GetManifestFilenames(fixturePath).First());
        }
    }
}
