using System;
using Freshli.Languages.Ruby;
using Xunit;

namespace Freshli.Test.Integration.Languages.Ruby {
  public class RubyGemsRepositoryTest {

    [Fact]
    public void VersionsBetween() {
    var repository = new RubyGemsRepository();
    var targetDate = new DateTime(2014, 04, 01, 0, 0, 0, DateTimeKind.Utc);
    var earlierVersion = new RubyGemsVersionInfo {Version = "0.3.38"};
    var laterVersion = new RubyGemsVersionInfo {Version = "1.1.0"};

    var versions = repository.VersionsBetween("tzinfo", targetDate,
      earlierVersion, laterVersion);

    Assert.Equal(3, versions.Count);
    }
  }

}
