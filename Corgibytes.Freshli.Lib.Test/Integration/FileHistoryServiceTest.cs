using System.IO;
using Corgibytes.Freshli.Lib.Exceptions;
using Xunit;

namespace Corgibytes.Freshli.Lib.Test.Integration
{
    public class FileHistoryServiceTest
    {
        [Fact]
        public void Git()
        {
            var rubyFixturePath = Fixtures.Path("ruby", "nokotest");

            var fileHistoryFinderRegistry = new FileHistoryFinderRegistry();
            fileHistoryFinderRegistry.Register<GitFileHistoryFinder>();
            var service = new FileHistoryService(fileHistoryFinderRegistry);
            var finder = service.SelectFinderFor(rubyFixturePath);

            Assert.IsType<GitFileHistory>(finder.FileHistoryOf(rubyFixturePath, "Gemfile.lock"));
        }

        [Fact]
        public void Local()
        {
            var emtpyFixturePath = Fixtures.Path("empty");

            var fileHistoryFinderRegistry = new FileHistoryFinderRegistry();
            fileHistoryFinderRegistry.Register<LocalFileHistoryFinder>();
            var service = new FileHistoryService(fileHistoryFinderRegistry);
            var finder = service.SelectFinderFor(emtpyFixturePath);

            Assert.IsType<LocalFileHistory>(finder.FileHistoryOf(emtpyFixturePath, "readme.md"));
        }

        [Fact]
        public void ThrowsWhenNoFinderIsAvailable()
        {
            var emtpyFixturePath = Fixtures.Path("empty");

            var fileHistoryFinderRegistry = new FileHistoryFinderRegistry();
            var service = new FileHistoryService(fileHistoryFinderRegistry);

            Assert.Throws<FileHistoryFinderNotFoundException>(() => service.SelectFinderFor(emtpyFixturePath));
        }
    }
}
