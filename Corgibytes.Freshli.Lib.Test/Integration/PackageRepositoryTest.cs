using System;
using Corgibytes.Freshli.Lib.Languages.Perl;
using Corgibytes.Freshli.Lib.Languages.Php;
using Corgibytes.Freshli.Lib.Languages.Ruby;
using Xunit;

namespace Corgibytes.Freshli.Lib.Test.Integration {
  public class PackageRepositoryTest {
    [Theory]
    [InlineData(
      typeof(MetaCpanRepository),
      new[] {"Plack", "1.0026"},
      new[] {2013, 06, 13, 06, 01, 17},
      "1.0026"
    )]
    [InlineData(
      typeof(MetaCpanRepository),
      new[] {"Test::More", "1.301001_048"},
      new[] {2014, 09, 25, 03, 39, 01},
      "1.301001_048"
    )]
    [InlineData(
       typeof(RubyGemsRepository),
       new[] {"tzinfo", "1.2.7"},
       new[] {2020, 04, 02, 21, 42, 11},
       "1.2.7"
     ),
     InlineData(
       typeof(RubyGemsRepository),
       new[] {"git", "1.6.0.pre1"},
       new[] {2020, 01, 20, 20, 50, 43},
       "1.6.0.pre1"
     )]
    [InlineData(
      typeof(MulticastComposerRepository),
      new[] {"monolog/monolog", "1.11.0"},
      new[] {2014, 09, 30, 13, 30, 58},
      "1.11.0"
    )]
    public void VersionInfo(
      Type repositoryType,
      string[] methodParams,
      int[] expectedDateParts,
      string expectedVersion
    ) {
      var gemName = methodParams[0];
      var gemVersion = methodParams[1];
      var repository = CreateRepository(repositoryType);
      var versionInfo = repository.VersionInfo(gemName, gemVersion);
      var expectedDate =
        DateBuilder.BuildDateTimeOffsetFromParts(expectedDateParts);

      Assert.Equal(expectedVersion, versionInfo.Version);
      Assert.Equal(expectedDate, versionInfo.DatePublished);
    }

    private static IPackageRepository CreateRepository(Type repositoryType) {
      if (repositoryType != typeof(MulticastComposerRepository)) {
        return (IPackageRepository) Activator.CreateInstance(repositoryType);
      }

      var phpFixturePath = Fixtures.Path("php", "small");
      var fileFinder = new FileHistoryFinder(phpFixturePath);
      return new MulticastComposerRepository(
        phpFixturePath,
        fileFinder.Finder
      );
    }

    [Theory]
    [InlineData(
      typeof(RubyGemsRepository),
      new object[] {"git", new[] {2020, 02, 01, 00, 00, 00}, false},
      "1.5.0",
      new[] {2018, 08, 10, 07, 58, 25}
    )]
    [InlineData(
      typeof(RubyGemsRepository),
      new object[] {"git", new[] {2020, 02, 01, 00, 00, 00}, true},
      "1.6.0.pre1",
      new[] {2020, 01, 20, 20, 50, 43}
    )]
    [InlineData(
      typeof(MetaCpanRepository),
      new object[] {"Plack", new[] {2018, 01, 01, 0, 0, 0}, false},
      "1.0045",
      new[] {2017, 12, 31, 20, 42, 50}
    )]
    [InlineData(
      typeof(MetaCpanRepository),
      new object[] {"Plack", new[] {2018, 01, 01, 0, 0, 0}, "1.0"},
      "1.0045",
      new[] {2017, 12, 31, 20, 42, 50}
    )]
    [InlineData(
      typeof(MetaCpanRepository),
      new object[] {"JSON", new[] {2018, 01, 01, 0, 0, 0}, ">= 2.00, < 2.80"},
      "2.61",
      new[] {2013, 10, 17, 11, 03, 11}
    )]
    [InlineData(
      typeof(MetaCpanRepository),
      new object[] {
        "Test::More", new[] {2018, 01, 01, 0, 0, 0}, ">= 0.96, < 2.0"
      },
      "1.301001_050",
      new[] {2014, 09, 26, 05, 44, 26}
    )]
    [InlineData(
      typeof(MulticastComposerRepository),
      new object[] {"monolog/monolog", new[] {2020, 01, 01, 00, 00, 00}, false},
      "2.0.2",
      new[] {2019, 12, 20, 14, 22, 00}
    )]
    [InlineData(
      typeof(MulticastComposerRepository),
      new object[] {
        "symfony/css-selector", new[] {2020, 01, 01, 00, 00, 00}, false
      },
      "v5.0.2",
      new[] {2019, 11, 18, 17, 27, 00}
    )]
    public void Latest(
      Type repositoryType,
      object[] methodParams,
      string expectedVersion,
      int[] expectedDateParts
    ) {
      var repository = CreateRepository(repositoryType);

      IVersionInfo versionInfo;
      if (methodParams[2] is bool) {
        versionInfo = CallLatestWithPreReleaseCheck(methodParams, repository);
      } else {
        versionInfo = CallLatestWithMatchExpression(methodParams, repository);
      }

      var expectedDate =
        DateBuilder.BuildDateTimeOffsetFromParts(expectedDateParts);

      Assert.Equal(expectedVersion, versionInfo.Version);
      Assert.Equal(expectedDate, versionInfo.DatePublished);
    }

    private static IVersionInfo CallLatestWithMatchExpression(
      object[] methodParams,
      IPackageRepository repository
    ) {
      var gemName = (string) methodParams[0];
      var matchExrpession = (string) methodParams[2];
      var targetDate =
        DateBuilder.BuildDateTimeOffsetFromParts((int[]) methodParams[1]);
      var versionInfo = repository.Latest(
        gemName,
        targetDate,
        matchExrpession
      );
      return versionInfo;
    }

    private static IVersionInfo CallLatestWithPreReleaseCheck(
      object[] methodParams,
      IPackageRepository repository
    ) {
      var gemName = (string) methodParams[0];
      var includePreReleases = (bool) methodParams[2];
      var targetDate =
        DateBuilder.BuildDateTimeOffsetFromParts((int[]) methodParams[1]);
      var versionInfo = repository.Latest(
        gemName,
        targetDate,
        includePreReleases
      );
      return versionInfo;
    }

    [Theory]
    [InlineData(
      typeof(RubyGemsRepository),
      typeof(RubyGemsVersionInfo),
      new object[] {
        "tzinfo", new[] {2014, 04, 01, 00, 00, 00}, "0.3.38", "1.1.0", true
      },
      3
    )]
    [InlineData(
      typeof(RubyGemsRepository),
      typeof(RubyGemsVersionInfo),
      new object[] {
        "google-protobuf",
        new[] {2020, 09, 01, 00, 00, 00},
        "3.11.0",
        "3.13.0",
        false
      },
      8
    )]
    [InlineData(
      typeof(RubyGemsRepository),
      typeof(RubyGemsVersionInfo),
      new object[] {
        "google-protobuf",
        new[] {2020, 09, 01, 00, 00, 00},
        "3.11.0",
        "3.13.0",
        true
      },
      11
    )]
    [InlineData(
      typeof(MetaCpanRepository),
      typeof(SemVerVersionInfo),
      new object[] {
        "Plack", new[] {2015, 01, 01, 00, 00, 00}, "1.0027", "1.0045", false
      },
      6
    )]
    public void VersionsBetween(
      Type repositoryType,
      Type versionInfoType,
      object[] methodParams,
      int expectedVersionCount
    ) {
      var targetDate =
        DateBuilder.BuildDateTimeOffsetFromParts((int[]) methodParams[1]);
      var earlierVersion = (IVersionInfo) Activator.CreateInstance(
        versionInfoType,
        methodParams[2],
        null
      );
      var laterVersion = (IVersionInfo) Activator.CreateInstance(
        versionInfoType,
        methodParams[3],
        null
      );

      var gemName = (string) methodParams[0];
      var includePreReleases = (bool) methodParams[4];
      var repository = CreateRepository(repositoryType);
      var versions = repository.VersionsBetween(
        gemName,
        targetDate,
        earlierVersion,
        laterVersion,
        includePreReleases
      );

      Assert.Equal(expectedVersionCount, versions.Count);
    }
  }
}
