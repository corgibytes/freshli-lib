using Freshli.Languages.JavaScript;
using Xunit;

namespace Freshli.Test.Unit.JavaScript {
  public class YarnLockfileReaderTest {
    [Fact]
    public void ReadComment() {
      var contents = "# test";
      var reader = new YarnLockfileReader(contents);

      Assert.Equal(YarnLockfileTokenType.None, reader.CurrentTokenType);
      Assert.Null(reader.CurrentTokenValue);

      reader.Read();
      Assert.Equal(YarnLockfileTokenType.Comment, reader.CurrentTokenType);
      Assert.Equal(" test", reader.CurrentTokenValue);

      reader.Read();
      Assert.Equal(YarnLockfileTokenType.Eof, reader.CurrentTokenType);
      Assert.Null(reader.CurrentTokenValue);
    }

    [Fact]
    public void ReadMultipleComments() {
      var contents = "# first line\n# second line";
      var reader = new YarnLockfileReader(contents);

      Assert.Equal(YarnLockfileTokenType.None, reader.CurrentTokenType);
      Assert.Null(reader.CurrentTokenValue);

      reader.Read();
      Assert.Equal(YarnLockfileTokenType.Comment, reader.CurrentTokenType);
      Assert.Equal(" first line", reader.CurrentTokenValue);

      reader.Read();
      Assert.Equal(YarnLockfileTokenType.Newline, reader.CurrentTokenType);
      Assert.Null(reader.CurrentTokenValue);

      reader.Read();
      Assert.Equal(YarnLockfileTokenType.Comment, reader.CurrentTokenType);
      Assert.Equal(" second line", reader.CurrentTokenValue);

      reader.Read();
      Assert.Equal(YarnLockfileTokenType.Eof, reader.CurrentTokenType);
      Assert.Null(reader.CurrentTokenValue);
    }


    [Fact]
    public void ReadObjectStart() {
      var contents = "\"@babel/cli@^7.8.0\":";
      var reader = new YarnLockfileReader(contents);

      Assert.Equal(YarnLockfileTokenType.None, reader.CurrentTokenType);
      Assert.Null(reader.CurrentTokenValue);

      reader.Read();
      Assert.Equal(YarnLockfileTokenType.String, reader.CurrentTokenType);
      Assert.Equal("@babel/cli@^7.8.0", reader.CurrentTokenValue);

      reader.Read();
      Assert.Equal(YarnLockfileTokenType.Colon, reader.CurrentTokenType);
      Assert.Null(reader.CurrentTokenValue);

      reader.Read();
      Assert.Equal(YarnLockfileTokenType.Eof, reader.CurrentTokenType);
      Assert.Null(reader.CurrentTokenValue);
    }
  }
}
