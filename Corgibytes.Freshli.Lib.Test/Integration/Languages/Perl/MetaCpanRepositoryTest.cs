using System;
using System.Collections.Generic;
using Corgibytes.Freshli.Lib.Languages.Perl;
using Xunit;

namespace Corgibytes.Freshli.Lib.Test.Integration.Languages.Perl
{
    public class MetaCpanRepositoryTest : RepositoryTestFixture<MetaCpanRepositoryTest>
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

        public override TheoryData<IList<object>, string, IList<int>> DataForTestingLatestWithOptionalPreRelease => new()
        {
            {
                new object[] { "Plack", new[] { 2018, 01, 01, 0, 0, 0 }, false },
                "1.0045",
                new[] { 2017, 12, 31, 20, 42, 50 }
            }

        };

        public override TheoryData<IList<object>, string, IList<int>> DataForTestingLatestWithMatchExpression => new()
        {
            {
                new object[] { "Plack", new[] { 2018, 01, 01, 0, 0, 0 }, "1.0" },
                "1.0045",
                new[] { 2017, 12, 31, 20, 42, 50 }
            },
            {
                new object[] { "JSON", new[] { 2018, 01, 01, 0, 0, 0 }, ">= 2.00, < 2.80" },
                "2.61",
                new[] { 2013, 10, 17, 11, 03, 11 }
            },
            {
                new object[] { "Test::More", new[] { 2018, 01, 01, 0, 0, 0}, ">= 0.96, < 2.0" },
                "1.301001_050",
                new[] { 2014, 09, 26, 05, 44, 26 }
            }

        };

        public override TheoryData<IList<object>, int> DataForTestingVersionsBetween => new()
        {
            {
                new object[]
                {
                    "Plack",
                    new[] {2015, 01, 01, 00, 00, 00},
                    new SemVerVersionInfo("1.0027"),
                    new SemVerVersionInfo("1.0045"),
                    false
                },
                6
            }
        };

    }
}
