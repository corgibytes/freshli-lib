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

	    [Theory(Skip="Skipping")]
        [InlineData(null, null, null)]
        public override void LatestWithMatchExpression(
          object[] _methodParams,
          string _expectedVersion,
          int[] _expectedDateParts
        )
        {
            // Initially empty method body. And all attributes removed to
            // prevent the test from running. The `RubyGemsRepository` doesn't
            // implement this overloaded form of the `Latest` method.
        }





    }
}
