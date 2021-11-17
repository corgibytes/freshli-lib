using System;
using System.Linq;
using Corgibytes.Freshli.Lib.Languages.CSharp;
using NuGet.Versioning;
using Xunit;

namespace Corgibytes.Freshli.Lib.Test.Unit.Languages.CSharp
{
    public class NuGetManifestParserTest
    {
        [Fact]
        public void ParsesFile()
        {

            var testContent = @"<Project Sdk=""Microsoft.NET.Sdk"">
            <ItemGroup>
            <PackageReference Include=""DotNetEnv"" Version=""1.4.0"" />
            <PackageReference Include=""Elasticsearch.Net"" Version=""7.10"" />
            <PackageReference Include=""HtmlAgilityPack"" Version=""1.11.30"" />
            <PackageReference Include=""LibGit2Sharp"" Version=""0.27.0"" />
            <PackageReference Include=""NLog"" Version=""4.7.7"" />
            <PackageReference Include=""RestSharp"" Version=""106.11.7"" />
            </ItemGroup>
        </Project>";

            var stream = Fixtures.CreateStream(testContent);

            var parser = new NuGetManifestParser();
            var manifest = parser.Parse(stream);

            Assert.Equal(6, manifest.Count());
        }
    }
}
