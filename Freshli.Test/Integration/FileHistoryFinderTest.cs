using System.IO;
using Xunit;

namespace Freshli.Test.Integration {
  public class FileHistoryFinderTest {
    [Fact]
    public void Git() {
      var rubyFixturePath = Fixtures.Path("ruby", "nokotest");

      FileHistoryFinder.Register<GitFileHistoryFinder>();
      var finder = new FileHistoryFinder(rubyFixturePath);

      Assert.IsType<GitFileHistory>(finder.FileHistoryOf("Gemfile.lock"));
    }

    [Fact]
    public void Default() {
      var emtpyFixturePath = Fixtures.Path("empty");

      var finder = new FileHistoryFinder(emtpyFixturePath);

      Assert.IsType<LocalFileHistory>(finder.FileHistoryOf("readme.md"));
    }
  }
}
