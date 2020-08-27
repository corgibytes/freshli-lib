using System.Collections.Generic;
using System.IO;
using Freshli.Languages.JavaScript;
using Xunit;

namespace Freshli.Test.Unit.JavaScript {
  public class YarkLockfileDocumentTest {
    [Fact]
    public void ReadSimpleDocumentWithoutEnumerator() {
      var contents = File.ReadAllText(
        Fixtures.Path("javascript", "yarn-simple", "yarn.lock")
      );
      var document = new YarnLockfileDocument(contents);

      Assert.Equal("1", document.FormatVersion);

      var root = document.RootElement;

      var ansiRegex = root.GetProperty("ansi-regex");
      Assert.Equal("ansi-regex", ansiRegex.Name);
      Assert.Equal("version", ansiRegex.Value.GetProperty("version").Name);
      Assert.Equal("5.0.0", ansiRegex.Value.GetProperty("version").Value.GetString());

      var normalizeUrl = root.GetProperty("normalize-url");
      Assert.Equal("normalize-url", normalizeUrl.Name);
      Assert.Equal("version", normalizeUrl.Value.GetProperty("version").Name);
      Assert.Equal("5.1.0", normalizeUrl.Value.GetProperty("version").Value.GetString());

      var stripAnsi = root.GetProperty("strip-ansi");
      Assert.Equal("strip-ansi", stripAnsi.Name);
      Assert.Equal("version", stripAnsi.Value.GetProperty("version").Name);
      Assert.Equal("6.0.0", stripAnsi.Value.GetProperty("version").Value.GetString());
    }

    [Fact]
    public void ReadSimpleDocument() {
      var contents = File.ReadAllText(
        Fixtures.Path("javascript", "yarn-simple", "yarn.lock")
      );
      var document = new YarnLockfileDocument(contents);

      Assert.Equal("1", document.FormatVersion);

      var dependencies = new Dictionary<string, string>();
      foreach (var dependency in document.RootElement.EnumerateObject()) {
        var version = dependency.Value.GetProperty("version").Value.GetString();

        dependencies[dependency.Name] = version;
      }

      Assert.Equal("5.0.0", dependencies["ansi-regex"]);
      Assert.Equal("5.1.0", dependencies["normalize-url"]);
      Assert.Equal("6.0.0", dependencies["strip-ansi"]);
    }
  }
}
