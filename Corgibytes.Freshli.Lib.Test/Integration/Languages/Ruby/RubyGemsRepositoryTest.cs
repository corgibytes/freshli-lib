using System;
using System.Collections.Generic;
using Corgibytes.Freshli.Lib.Languages.Ruby;
using Xunit;

namespace Corgibytes.Freshli.Lib.Test.Integration.Languages.Ruby
{
    public class RubyGemsRepositoryTest : RepositoryTestFixture<RubyGemsRepositoryTest>
    {
        public override IPackageRepository Repository => new RubyGemsRepository();

        public override TheoryData<IList<string>, IList<int>, string> DataForTestingVersionInfo => new()
        {
            {
                new[] { "tzinfo", "1.2.7" },
                new[] { 2020, 04, 02, 21, 42, 11 },
                "1.2.7"
            },
            {
                new[] { "git", "1.6.0.pre1" },
                new[] { 2020, 01, 20, 20, 50, 43 },
                "1.6.0.pre1"
            }
        };

        public override TheoryData<IList<object>, string, IList<int>> DataForTestingLatestWithOptionalPreRelease => new()
        {
            {
                new object[] { "git", new[] { 2020, 02, 01, 00, 00, 00 }, false },
                "1.5.0",
                new[] { 2018, 08, 10, 07, 58, 25 }
            },
            {
                new object[] { "git", new[] { 2020, 02, 01, 00, 00, 00 }, true },
                "1.6.0.pre1",
                new[] { 2020, 01, 20, 20, 50, 43 }
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
            // Intentionally empty method body. And all attributes removed to
            // prevent the test from running. The `RubyGemsRepository` doesn't
            // implement this overloaded form of the `Latest` method.
        }

        public override TheoryData<IList<object>, int> DataForTestingVersionsBetween => new()
        {
            {
                new object[] {
                    "tzinfo",
                    new[] {2014, 04, 01, 00, 00, 00},
                    new RubyGemsVersionInfo("0.3.38"),
                    new RubyGemsVersionInfo("1.1.0"),
                    true
                },
                3
            },
            {
                new object[] {
                    "google-protobuf",
                    new[] {2020, 09, 01, 00, 00, 00},
                    new RubyGemsVersionInfo("3.11.0"),
                    new RubyGemsVersionInfo("3.13.0"),
                    false
                },
                8
            },
            {
                new object[] {
                    "google-protobuf",
                    new[] {2020, 09, 01, 00, 00, 00},
                    new RubyGemsVersionInfo("3.11.0"),
                    new RubyGemsVersionInfo("3.13.0"),
                    true
                },
                11
            }
        };
    }
}
