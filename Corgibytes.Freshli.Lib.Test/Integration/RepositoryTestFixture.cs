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

        public abstract TheoryData<IList<string>, IList<int>, string> DataForTestingVersionInfo { get; }

        [Theory]
        [InstanceMemberData(nameof(DataForTestingVersionInfo))]
        public void VersionInfo(
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

    }
}
