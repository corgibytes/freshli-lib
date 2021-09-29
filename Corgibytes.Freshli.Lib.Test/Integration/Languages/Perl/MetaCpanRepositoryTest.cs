using System;
using System.Collections.Generic;
using Corgibytes.Freshli.Lib.Languages.Perl;
using Xunit;

namespace Corgibytes.Freshli.Lib.Test.Integration.Languages.Perl
{
    public class MetaCpanRepositoryTest: RepositoryTestFixture<MetaCpanRepositoryTest>
    {
        public override IPackageRepository Repository => new MetaCpanRepository();

        public override TheoryData<IList<string>, IList<int>, string> DataForTestingVersionInfo => new()
        {
            {
                new[] { "Plack", "1.0026" },
                new[] { 2013, 06, 13, 06, 01, 17 },
                "1.0026"
            },
            {
                new[] { "Test::More", "1.301001_048" },
                new[] { 2014, 09, 25, 03, 39, 01 },
                "1.301001_048"
            }
        };

    }
}
