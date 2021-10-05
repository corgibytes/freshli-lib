using System;
using System.Collections.Generic;
using Corgibytes.Freshli.Lib;
using Corgibytes.Xunit.Extensions;
using Xunit;

namespace Corgibytes.Freshli.Lib.Test.Integration
{
    public abstract class RepositoryTestFixture<T>
    {
        public abstract IPackageRepository Repository { get; }

        public virtual TheoryData<IList<string>, IList<int>, string> DataForTestingVersionInfo => new() { };

        [Theory]
        [InstanceMemberData(nameof(DataForTestingVersionInfo))]
        public virtual void VersionInfo(
            string[] methodParams,
            int[] expectedDateParts,
            string expectedVersion
        )
        {
            var gemName = methodParams[0];
            var gemVersion = methodParams[1];
            var versionInfo = Repository.VersionInfo(gemName, gemVersion);
            var expectedDate =
                DateBuilder.BuildDateTimeOffsetFromParts(expectedDateParts);

            Assert.Equal(expectedVersion, versionInfo.Version);
            Assert.Equal(expectedDate, versionInfo.DatePublished);
        }


        public virtual TheoryData<IList<object>, string, IList<int>> DataForTestingLatestWithOptionalPreRelease => new() { };

        [Theory]
        [InstanceMemberData(nameof(DataForTestingLatestWithOptionalPreRelease))]
        public virtual void LatestWithOptionalPreRelease(
            object[] methodParams,
            string expectedVersion,
            int[] expectedDateParts
        )
        {
            var packageName = (string)methodParams[0];
            var asOf = DateBuilder.BuildDateTimeOffsetFromParts((int[])methodParams[1]);
            var includePreReleases = (bool)methodParams[2];

            IVersionInfo versionInfo = Repository.Latest(packageName, asOf, includePreReleases);

            var expectedDate =
                DateBuilder.BuildDateTimeOffsetFromParts(expectedDateParts);

            Assert.Equal(expectedVersion, versionInfo.Version);
            Assert.Equal(expectedDate, versionInfo.DatePublished);
        }

        public virtual TheoryData<IList<object>, string, IList<int>> DataForTestingLatestWithMatchExpression => new() { };

        [Theory]
        [InstanceMemberData(nameof(DataForTestingLatestWithMatchExpression))]
        public virtual void LatestWithMatchExpression(
            object[] methodParams,
            string expectedVersion,
            int[] expectedDateParts
        )
        {
            var packageName = (string)methodParams[0];
            var asOf = DateBuilder.BuildDateTimeOffsetFromParts((int[])methodParams[1]);
            var matchExpression = (string)methodParams[2];

            IVersionInfo versionInfo = Repository.Latest(packageName, asOf, matchExpression);

            var expectedDate =
                DateBuilder.BuildDateTimeOffsetFromParts(expectedDateParts);

            Assert.Equal(expectedVersion, versionInfo.Version);
            Assert.Equal(expectedDate, versionInfo.DatePublished);
        }

        public virtual TheoryData<IList<object>, int> DataForTestingVersionsBetween => new() { };

        [Theory]
        [InstanceMemberData(nameof(DataForTestingVersionsBetween))]
        public virtual void VersionsBetween(object[] methodParams, int expectedVersionCount)
        {
            var targetDate =
                DateBuilder.BuildDateTimeOffsetFromParts((int[])methodParams[1]);
            var earlierVersion = (IVersionInfo)methodParams[2];
            var laterVersion = (IVersionInfo)methodParams[3];

            var gemName = (string)methodParams[0];
            var includePreReleases = (bool)methodParams[4];
            var versions = Repository.VersionsBetween(
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
