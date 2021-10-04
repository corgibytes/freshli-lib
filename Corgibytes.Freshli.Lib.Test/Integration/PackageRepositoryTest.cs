using System;
using System.Collections.Generic;
using System.Reflection;
using Corgibytes.Freshli.Lib.Languages.Perl;
using Corgibytes.Freshli.Lib.Languages.Php;
using Corgibytes.Freshli.Lib.Languages.Ruby;
using Elasticsearch.Net;
using Xunit;

namespace Corgibytes.Freshli.Lib.Test.Integration
{
    public class PackageRepositoryTest
    {
        private static IPackageRepository CreateRepository(Type repositoryType)
        {
            if (repositoryType != typeof(MulticastComposerRepository))
            {
                return (IPackageRepository)Activator.CreateInstance(repositoryType);
            }

            var phpFixturePath = Fixtures.Path("php", "small");
            var fileFinder = new FileHistoryFinder(phpFixturePath);
            return new MulticastComposerRepository(
              phpFixturePath,
              fileFinder.Finder
            );
        }

        private static IVersionInfo InvokeLatest(
          IPackageRepository repository,
          object[] methodParams
        )
        {
            var cleanedParameters = PrepareParameters(methodParams);
            return (IVersionInfo)repository.GetType().InvokeMember(
              "Latest",
              BindingFlags.InvokeMethod | BindingFlags.OptionalParamBinding,
              Type.DefaultBinder,
              repository,
              cleanedParameters
            );
        }

        private static object[] PrepareParameters(
          object[] methodParams
        )
        {
            return new[] {
        methodParams[0],
        DateBuilder.BuildDateTimeOffsetFromParts((int[]) methodParams[1]),
        methodParams[2]
      };
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
        )
        {
            var targetDate =
              DateBuilder.BuildDateTimeOffsetFromParts((int[])methodParams[1]);
            var earlierVersion = (IVersionInfo)Activator.CreateInstance(
              versionInfoType,
              methodParams[2],
              null
            );
            var laterVersion = (IVersionInfo)Activator.CreateInstance(
              versionInfoType,
              methodParams[3],
              null
            );

            var gemName = (string)methodParams[0];
            var includePreReleases = (bool)methodParams[4];
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
