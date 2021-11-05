using System.IO;
using Xunit;

namespace Corgibytes.Freshli.Lib.Test.Integration
{
    public class FileHistoryServiceTest
    {
        [Fact]
        public void Git()
        {
            var rubyFixturePath = Fixtures.Path("ruby", "nokotest");

            FileHistoryFinderRegistry.Register<GitFileHistoryFinder>();
            var service = new FileHistoryService();
            var finder = service.SelectFinderFor(rubyFixturePath);

            Assert.IsType<GitFileHistory>(finder.FileHistoryOf(rubyFixturePath, "Gemfile.lock"));
        }

        [Fact]
        public void Default()
        {
            var emtpyFixturePath = Fixtures.Path("empty");

            var service = new FileHistoryService();
            var finder = service.SelectFinderFor(emtpyFixturePath);

            Assert.IsType<LocalFileHistory>(finder.FileHistoryOf(emtpyFixturePath, "readme.md"));
        }
    }
}
