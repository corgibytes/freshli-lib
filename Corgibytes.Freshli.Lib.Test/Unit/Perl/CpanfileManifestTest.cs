﻿using System.IO;
using Corgibytes.Freshli.Lib.Languages.Perl;
using Xunit;

namespace Corgibytes.Freshli.Lib.Test.Unit.Perl {
  public class CpanfileManifestTest {
    [Fact]
    public void Parse() {
      var contents = File.ReadAllText(
        Fixtures.Path(
          "perl",
          "cpanfile",
          "simple-without-snapshot",
          "cpanfile"
        )
      );
      var manifest = new CpanfileManifest();
      manifest.Parse(contents);

      Assert.Equal(3, manifest.Count);

      Assert.Equal("1.0", manifest["Plack"].Version);
      Assert.Equal(">= 2.00, < 2.80", manifest["JSON"].Version);
      Assert.Equal(">= 0.96, < 2.0", manifest["Test::More"].Version);
    }

    [Fact]
    public void ParseImpliesClear() {
      var contents = File.ReadAllText(
        Fixtures.Path(
          "perl",
          "cpanfile",
          "simple-without-snapshot",
          "cpanfile"
        )
      );
      var manifest = new CpanfileManifest();
      manifest.Parse(contents);
      manifest.Add("erase", "me");

      manifest.Parse(contents);

      Assert.Equal(3, manifest.Count);

      Assert.Equal("1.0", manifest["Plack"].Version);
      Assert.Equal(">= 2.00, < 2.80", manifest["JSON"].Version);
      Assert.Equal(">= 0.96, < 2.0", manifest["Test::More"].Version);
    }
  }
}
