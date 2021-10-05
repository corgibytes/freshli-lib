using System;
using System.Collections.Generic;
using Corgibytes.Freshli.Lib.Languages.Python;
using Xunit;

namespace Corgibytes.Freshli.Lib.Test.Integration.Languages.Python
{
    public class PyPIRepositoryTest : RepositoryTestFixture<PyPIRepositoryTest>
    {
        public override IPackageRepository Repository => new PyPIRepository();

        public override TheoryData<IList<string>, IList<int>, string> DataForTestingVersionInfo => new()
        {
            {
                new[] { "numpy", "0.9.6" },
                new[] { 2006, 03, 14, 10, 11, 55 },
                "0.9.6"
            }
        };

        public override TheoryData<IList<object>, string, IList<int>> DataForTestingLatestWithOptionalPreRelease => new()
        {
            {
                new object[] { "numpy", new[] { 2020, 01, 01, 00, 00, 00 }, false },
                "1.18.0",
                new[] { 2019, 12, 22, 15, 32, 32 }
            }
        };

        public override TheoryData<IList<object>, string, IList<int>> DataForTestingLatestWithMatchExpression => new()
        {
            {
                new object[] { "numpy", new[] { 2020, 01, 01, 00, 00, 00 }, "==1.16.*" },
                "1.16.6",
                new[] { 2019, 12, 29, 22, 23, 23 }
            },
            {
                new object[] { "matplotlib", new[] { 2020, 01, 01, 00, 00, 00 }, "==3.*" },
                "3.1.2",
                new[] { 2019, 11, 21, 22, 51, 38 }
            },
            {
                new object[] { "seaborn", new[] { 2020, 01, 01, 00, 00, 00 }, "==0.8.1" },
                "0.8.1",
                new[] { 2017, 09, 03, 16, 38, 23 }
            }
        };

        public override TheoryData<IList<object>, int> DataForTestingVersionsBetween => new()
        {
            {
                new object[]
                {
                    "pymongo",
                    new[] {2015, 12, 01, 00, 00, 00},
                    new PythonVersionInfo("2.9"),
                    new PythonVersionInfo("3.0.3"),
                    false
                },
                4
            }
        };
    }
}
