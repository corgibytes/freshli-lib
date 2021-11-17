using System.Linq;
using System.IO;
using Corgibytes.Freshli.Lib.Languages.Python;
using Xunit;

namespace Corgibytes.Freshli.Lib.Test.Unit.Languages.Python
{
    public class PipRequirementsTxtManifestParserTest
    {
        private static readonly string Contents = File.
          ReadAllText(
            Fixtures.Path(
              "python",
              "requirements-txt",
              "small",
              "requirements.txt"
            )
          );

        [Fact]
        public void Parse()
        {
            var stream = Fixtures.CreateStream(Contents);
            var parser = new PipRequirementsTxtManifestParser();
            var packages = parser.Parse(stream);

            var manifest = packages.ToDictionary(p => p.Name);
            Assert.Equal(9, manifest.Count);
            Assert.Equal("==1.16.*", manifest["numpy"].Version);
            Assert.Equal("==3.*", manifest["matplotlib"].Version);
            Assert.Equal("==0.8.1", manifest["seaborn"].Version);
            Assert.Equal(">=1.5", manifest["six"].Version);
            Assert.Equal(">1.0.1", manifest["kiwisolver"].Version);
            Assert.Equal("", manifest["pandas"].Version);
            Assert.Equal(">=2017.2,<2020.1", manifest["pytz"].Version);
            Assert.Equal("<1.4.1", manifest["scipy"].Version);
            Assert.Equal("==0.20", manifest["preshed"].Version);
        }

        [Fact]
        public void ParseCorrectlyHandlesPackageNameThatContainsDot()
        {
            var stream = Fixtures.CreateStream("backports.ssl-match-hostname==3.4.0.2");
            var parser = new PipRequirementsTxtManifestParser();
            var packages = parser.Parse(stream);

            var manifest = packages.ToDictionary(p => p.Name);

            Assert.Single(manifest);
            Assert.Equal("==3.4.0.2", manifest["backports.ssl-match-hostname"].Version);
        }

        [Fact]
        public void ParseRemovesWhiteSpaceFromVersion()
        {
            var stream = Fixtures.CreateStream("preshed == 0.20");
            var parser = new PipRequirementsTxtManifestParser();
            var packages = parser.Parse(stream);

            var manifest = packages.ToDictionary(p => p.Name);

            Assert.Single(manifest);
            Assert.Equal("==0.20", manifest["preshed"].Version);
        }
    }
}
