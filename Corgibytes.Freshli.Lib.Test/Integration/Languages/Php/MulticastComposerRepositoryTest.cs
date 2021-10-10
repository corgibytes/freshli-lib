using System;
using System.Collections.Generic;
using Corgibytes.Freshli.Lib.Languages.Php;
using Xunit;

namespace Corgibytes.Freshli.Lib.Test.Integration.Languages.Php
{
    public class MulticastComposerRepositoryTest : RepositoryTestFixture<MulticastComposerRepositoryTest>
    {
        public override IPackageRepository Repository
        {
            get
            {
                var phpFixturePath = Fixtures.Path("php", "small");
                var fileFinder = new FileHistoryFinder(phpFixturePath);
                return new MulticastComposerRepository(
                    phpFixturePath,
                    fileFinder.Finder
                );
            }
        }

        public override TheoryData<IList<string>, IList<int>, string> DataForTestingVersionInfo => new()
        {
            {
                new[] { "monolog/monolog", "1.11.0" },
                new[] { 2014, 09, 30, 13, 30, 58 },
                "1.11.0"
            }
        };

        public override TheoryData<IList<object>, string, IList<int>> DataForTestingLatestWithOptionalPreRelease => new()
        {
            {
                new object[] { "monolog/monolog", new[] { 2020, 01, 01, 00, 00, 00 }, false },
                "2.0.2",
                new[] { 2019, 12, 20, 14, 22, 59 }
            },
            {
                new object[] { "symfony/css-selector", new[] { 2020, 01, 01, 00, 00, 00 }, false },
                "v5.0.2",
                new[] { 2019, 11, 18, 17, 27, 11 }
            }
        };

        [Theory(Skip = "Skipping")]
        [InlineData(null, null, null)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Warning", "xUnit1026",
            Justification = "Inentionally empty method")]
        public override void LatestWithMatchExpression(
          object[] _methodParams,
          string _expectedVersion,
          int[] _expectedDateParts
        )
        {
            // Initially empty method body. The `MulticastComposerRepository` doesn't
            // implement this overloaded form of the `Latest` method.
        }

        [Theory(Skip = "Skipping")]
        [InlineData(null, 0)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Warning", "xUnit1026",
            Justification = "Inentionally empty method")]
        public override void VersionsBetween(object[] methodParams, int expectedVersionCount)
        {
            // Initially empty method body. The `MulticastComposerRepository` doesn't
            // implement this overloaded form of the `VersionsBetween` method.
        }

    }
}
