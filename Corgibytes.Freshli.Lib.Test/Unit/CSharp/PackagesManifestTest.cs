using Corgibytes.Freshli.Lib.Languages.CSharp;
using Xunit;

namespace Corgibytes.Freshli.Lib.Test.Unit.CSharp;

public class PackagesManifestTest
{
    [Fact]
    public void ParsesFile()
    {
        var manifest = new PackagesManifest();
        var testContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
            <packages>
            <package id=""DotNetEnv"" version=""1.4.0"" />
            <package id=""Elasticsearch.Net"" version=""7.10"" />
            <package id=""HtmlAgilityPack"" version=""1.11.30"" />
            <package id=""LibGit2Sharp"" version=""0.27.0"" />
            <package id=""NLog"" version=""4.7.7"" />
            <package id=""RestSharp"" version=""106.11.7"" />
            </packages>";

        manifest.Parse(testContent);

        Assert.Equal(6, manifest.Count);
    }

}
