using Xunit;

namespace Corgibytes.Freshli.Lib.Test.Integration {
  public class AssertIPackageRepository {
    public static void VerifyVersionInfo(
      IPackageRepository repository,
      string[] methodParams,
      int[] expectedDateParts,
      string expectedVersion
    ) {
      var gemName = methodParams[0];
      var gemVersion = methodParams[1];
      var versionInfo = repository.VersionInfo(gemName, gemVersion);
      var expectedDate =
        DateBuilder.BuildDateTimeOffsetFromParts(expectedDateParts);

      Assert.Equal(expectedVersion, versionInfo.Version);
      Assert.Equal(expectedDate, versionInfo.DatePublished);
    }
  }
}
