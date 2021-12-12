using System;
using Corgibytes.Freshli.Lib.Languages.Ruby;
using Xunit;

namespace Corgibytes.Freshli.Lib.Test.Integration.Languages.Ruby
{
    public class RubyBundlerManifestFinderTest
    {
        [Fact]
        public void GetManifests()
        {
            var fileHistoryFinder = new LocalFileHistoryFinder();
            var rubyFixturePath = Fixtures.Path("ruby", "nokotest");
            var fileHistorySource = fileHistoryFinder.HistorySourceFor(rubyFixturePath);

            var finder = new RubyBundlerManifestFinder();

            finder.GetManifests(fileHistorySource);
        }
    }
}
