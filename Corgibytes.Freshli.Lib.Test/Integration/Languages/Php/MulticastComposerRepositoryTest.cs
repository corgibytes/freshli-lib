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
    }
}
