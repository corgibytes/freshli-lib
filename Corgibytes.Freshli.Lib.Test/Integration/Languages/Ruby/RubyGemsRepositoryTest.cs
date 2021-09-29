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
    }
}
