using Freshli.Languages.Perl;
using Freshli.Languages.Php;
using Freshli.Languages.Python;
using Freshli.Languages.Ruby;
using Xunit;

namespace Freshli.Test.Integration {
  public class ManifestFinderTest {
    public ManifestFinderTest() {
      ManifestFinder.RegisterAll();
    }

    [Fact]
    public void Empty() {
      var emptyFixturePath = Fixtures.Path("empty");
      var fileFinder = new FileHistoryFinder(emptyFixturePath);
      var finder = new ManifestFinder(emptyFixturePath, fileFinder.Finder);
      Assert.False(finder.Successful);
    }

    [Fact]
    public void RubyBundler() {
      var rubyFixturePath = Fixtures.Path("ruby", "nokotest");

      var fileFinder = new FileHistoryFinder(rubyFixturePath);
      var finder = new ManifestFinder(rubyFixturePath, fileFinder.Finder);

      Assert.True(finder.Successful);
      Assert.Equal("Gemfile.lock", finder.LockFileName);

      Assert.IsType<RubyGemsRepository>(finder.Calculator.Repository);
      Assert.IsType<BundlerManifest>(finder.Calculator.Manifest);
    }

    [Fact]
    public void PhpComposer() {
      var phpFixturePath = Fixtures.Path("php", "small");

      var fileFinder = new FileHistoryFinder(phpFixturePath);
      var finder = new ManifestFinder(phpFixturePath, fileFinder.Finder);

      Assert.True(finder.Successful);
      Assert.Equal("composer.lock", finder.LockFileName);

      Assert.IsType<MulticastComposerRepository>(finder.Calculator.Repository);
      Assert.IsType<ComposerManifest>(finder.Calculator.Manifest);
    }

    [Fact]
    public void PythonPipRequirementsTxt() {
      var pythonFixturePath = Fixtures.Path(
        "python",
        "requirements-txt",
        "small"
      );

      var fileFinder = new FileHistoryFinder(pythonFixturePath);
      var finder = new ManifestFinder(pythonFixturePath, fileFinder.Finder);

      Assert.True(finder.Successful);
      Assert.Equal("requirements.txt", finder.LockFileName);

      Assert.IsType<PyPIRepository>(finder.Calculator.Repository);
      Assert.IsType<PipRequirementsTxtManifest>(finder.Calculator.Manifest);
    }


    [Fact]
    public void PerlCpanfile() {
      var fixturePath = Fixtures.Path(
        "perl",
        "cpanfile",
        "simple-without-snapshot"
      );

      var fileFinder = new FileHistoryFinder(fixturePath);
      var finder = new ManifestFinder(fixturePath, fileFinder.Finder);

      Assert.True(finder.Successful);
      Assert.Equal("cpanfile", finder.LockFileName);

      Assert.IsType<MetaCpanRepository>(finder.Calculator.Repository);
      Assert.IsType<CpanfileManifest>(finder.Calculator.Manifest);
    }
  }
}
