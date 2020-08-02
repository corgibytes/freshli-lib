using System.IO;
using ApprovalTests.Reporters;
using Freshli.Languages.JavaScript;
using Xunit;

namespace Freshli.Test.Unit.JavaScript {
  public class PackageJsonManifestTest {
    [Fact]
    public void Parse() {
      var contents = File.ReadAllText(
        Fixtures.Path(
          "javascript",
          "npm-no-lock",
          "package.json"
        )
      );

      var manifest = new PackageJsonManifest();
      manifest.Parse(contents);

      Assert.Equal(36, manifest.Count);

      Assert.Equal("^1.10.0", manifest["follow-redirects"].Version);
      Assert.Equal("^0.17.0", manifest["bundlesize"].Version);
      Assert.Equal("^3.0.0", manifest["coveralls"].Version);
      Assert.Equal("^4.2.4", manifest["es6-promise"].Version);
      Assert.Equal("^1.0.2", manifest["grunt"].Version);
      Assert.Equal("^0.6.0", manifest["grunt-banner"].Version);
      Assert.Equal("^1.2.0", manifest["grunt-cli"].Version);
      Assert.Equal("^1.1.0", manifest["grunt-contrib-clean"].Version);
      Assert.Equal("^1.0.0", manifest["grunt-contrib-watch"].Version);
      Assert.Equal("^20.1.0", manifest["grunt-eslint"].Version);
      Assert.Equal("^2.0.0", manifest["grunt-karma"].Version);
      Assert.Equal("^0.13.3", manifest["grunt-mocha-test"].Version);
      Assert.Equal("^6.0.0-beta.19", manifest["grunt-ts"].Version);
      Assert.Equal("^1.0.18", manifest["grunt-webpack"].Version);
      Assert.Equal("^1.0.0", manifest["istanbul-instrumenter-loader"].Version);
      Assert.Equal("^2.4.1", manifest["jasmine-core"].Version);
      Assert.Equal("^1.3.0", manifest["karma"].Version);
      Assert.Equal("^2.2.0", manifest["karma-chrome-launcher"].Version);
      Assert.Equal("^1.1.1", manifest["karma-coverage"].Version);
      Assert.Equal("^1.1.0", manifest["karma-firefox-launcher"].Version);
      Assert.Equal("^1.1.1", manifest["karma-jasmine"].Version);
      Assert.Equal("^0.1.13", manifest["karma-jasmine-ajax"].Version);
      Assert.Equal("^1.0.0", manifest["karma-opera-launcher"].Version);
      Assert.Equal("^1.0.0", manifest["karma-safari-launcher"].Version);
      Assert.Equal("^1.2.0", manifest["karma-sauce-launcher"].Version);
      Assert.Equal("^1.0.5", manifest["karma-sinon"].Version);
      Assert.Equal("^0.3.7", manifest["karma-sourcemap-loader"].Version);
      Assert.Equal("^1.7.0", manifest["karma-webpack"].Version);
      Assert.Equal("^3.5.2", manifest["load-grunt-tasks"].Version);
      Assert.Equal("^1.2.0", manifest["minimist"].Version);
      Assert.Equal("^5.2.0", manifest["mocha"].Version);
      Assert.Equal("^4.5.0", manifest["sinon"].Version);
      Assert.Equal("^2.8.1", manifest["typescript"].Version);
      Assert.Equal("^0.10.0", manifest["url-search-params"].Version);
      Assert.Equal("^1.13.1", manifest["webpack"].Version);
      Assert.Equal("^1.14.1", manifest["webpack-dev-server"].Version);

    }
  }
}
