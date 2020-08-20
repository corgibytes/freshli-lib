using Freshli.Languages.JavaScript;
using Xunit;

namespace Freshli.Test.Unit.JavaScript {
  public class YarnLockfileReaderTest {
    [Fact]
    public void ReadComment() {
      var contents = "# test";
      var reader = new YarnLockfileReader(contents);

      AssertTokenNone(reader);

      reader.Read();
      AssertTokenComment(reader, " test");

      reader.Read();
      AssertTokenEof(reader);
    }

    [Fact]
    public void ReadMultipleComments() {
      var contents = "# first line\n# second line";
      var reader = new YarnLockfileReader(contents);

      AssertTokenNone(reader);

      reader.Read();
      AssertTokenComment(reader, " first line");

      reader.Read();
      AssertTokenNewline(reader);

      reader.Read();
      AssertTokenComment(reader, " second line");

      reader.Read();
      AssertTokenEof(reader);
    }

    [Fact]
    public void ReadObjectStart() {
      var contents = "\"@babel/cli@^7.8.0\":";
      var reader = new YarnLockfileReader(contents);

      AssertTokenNone(reader);

      reader.Read();
      AssertTokenString(reader, value: "@babel/cli@^7.8.0");

      reader.Read();
      AssertTokenColon(reader);

      reader.Read();
      AssertTokenEof(reader);
    }

    private static void AssertTokenColon(YarnLockfileReader reader) {
      Assert.Equal(YarnLockfileTokenType.Colon, reader.CurrentTokenType);
      Assert.Null(reader.CurrentTokenValue);
    }

    private static void AssertTokenComment(YarnLockfileReader reader, string value) {
      Assert.Equal(YarnLockfileTokenType.Comment, reader.CurrentTokenType);
      Assert.Equal(value, reader.CurrentTokenValue);
    }

    private static void AssertTokenEof(YarnLockfileReader reader) {
      Assert.Equal(YarnLockfileTokenType.Eof, reader.CurrentTokenType);
      Assert.Null(reader.CurrentTokenValue);
    }

    private static void AssertTokenNewline(YarnLockfileReader reader) {
      Assert.Equal(YarnLockfileTokenType.Newline, reader.CurrentTokenType);
      Assert.Null(reader.CurrentTokenValue);
    }

    private static void AssertTokenNone(YarnLockfileReader reader) {
      Assert.Equal(YarnLockfileTokenType.None, reader.CurrentTokenType);
      Assert.Null(reader.CurrentTokenValue);
    }

    private static void AssertTokenString(YarnLockfileReader reader, string value) {
      Assert.Equal(YarnLockfileTokenType.String, reader.CurrentTokenType);
      Assert.Equal(value, reader.CurrentTokenValue);
    }
  }
}
