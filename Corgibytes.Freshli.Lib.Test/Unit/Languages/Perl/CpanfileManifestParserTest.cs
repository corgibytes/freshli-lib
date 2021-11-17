using System;
using System.Linq;
using System.IO;
using Corgibytes.Freshli.Lib.Languages.Perl;
using Xunit;

namespace Corgibytes.Freshli.Lib.Test.Unit.Languages.Perl
{
    public class CpanfileManifestParserTest
    {
        [Fact]
        public void Parse()
        {
            var contents = File.ReadAllText(
              Fixtures.Path(
                "perl",
                "cpanfile",
                "simple-without-snapshot",
                "cpanfile"
              )
            );
            var stream = Fixtures.CreateStream(contents);
            var parser = new CpanfileManifestParser();
            var packages = parser.Parse(stream);

            var manifest = packages.ToDictionary(p => p.Name);

            Assert.Equal(3, manifest.Count);
            Assert.Equal("1.0", manifest["Plack"].Version);
            Assert.Equal(">= 2.00, < 2.80", manifest["JSON"].Version);
            Assert.Equal(">= 0.96, < 2.0", manifest["Test::More"].Version);
        }
    }
}
