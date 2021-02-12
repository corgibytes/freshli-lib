using Freshli.Languages.Perl;
using Xunit;

namespace Freshli.Test.Unit.Perl {
  public class VersionMatcherTest {
    [Theory]
    [InlineData("1.0", "1.0", true)]
    [InlineData("1.1", "1.0", true)]
    [InlineData("0.9", "1.0", false)]
    [InlineData("2.0", "1.0", true)]
    [InlineData("1.1", "1.1", true)]
    [InlineData("1.2", "1.1", true)]
    [InlineData("1.0", "1.1", false)]
    [InlineData("1.1", "> 0.9", true)]
    [InlineData("0.9", "> 1.1", false)]
    [InlineData("1.1", ">= 0.9", true)]
    [InlineData("0.9", ">= 1.1", false)]
    [InlineData("2.0", "> 2.0", false)]
    [InlineData("2.0", ">= 2.0", true)]
    [InlineData("2.0", "< 2.0", false)]
    [InlineData("2.0", "<= 2.0", true)]
    [InlineData("1.1", "< 0.9", false)]
    [InlineData("1.1", "<= 0.9", false)]
    [InlineData("0.9", "< 1.1", true)]
    [InlineData("0.9", "<= 1.1", true)]
    [InlineData("0.9", "== 1.0", false)]
    [InlineData("1.0", "== 1.0", true)]
    [InlineData("0.9", "!= 1.0", true)]
    [InlineData("1.0", "!= 1.0", false)]
    [InlineData("1.5", ">= 1.2, != 1.5", false)]
    [InlineData("1.2", ">= 1.2, != 1.5", true)]
    [InlineData("2.0", ">= 1.2, != 1.5", true)]
    [InlineData("1.5", ">= 1.2, != 1.5, < 2.0", false)]
    [InlineData("1.2", ">= 1.2, != 1.5, < 2.0", true)]
    [InlineData("2.0", ">= 1.2, != 1.5, < 2.0", false)]
    public void DoesMatchHandleVersionSpecifierOperator(
      string version,
      string expression,
      bool doesMatch
    ) {
      var info = new SemVerVersionInfo(version);
      var matcher = VersionMatcher.Create(expression);
      Assert.Equal(doesMatch, matcher.DoesMatch(info));
    }

    [Theory]
    [InlineData("2.0", OperationKind.GreaterThanEqual, "2.0")]
    [InlineData(">= 2.0", OperationKind.GreaterThanEqual, "2.0")]
    [InlineData("== 2.0", OperationKind.Matching, "2.0")]
    [InlineData("<= 2.0", OperationKind.LessThanEqual, "2.0")]
    [InlineData("< 2.0", OperationKind.LessThan, "2.0")]
    [InlineData("> 2.0", OperationKind.GreaterThan, "2.0")]
    [InlineData("!= 2.0", OperationKind.NotEqual, "2.0")]
    [InlineData(">=2.0", OperationKind.GreaterThanEqual, "2.0")]
    [InlineData("==2.0", OperationKind.Matching, "2.0")]
    [InlineData("<=2.0", OperationKind.LessThanEqual, "2.0")]
    [InlineData("<2.0", OperationKind.LessThan, "2.0")]
    [InlineData(">2.0", OperationKind.GreaterThan, "2.0")]
    [InlineData("!=2.0", OperationKind.NotEqual, "2.0")]
    public void IdentifiesOperationAndVersion(
      string expression,
      OperationKind operation,
      string version
    ) {
      var matcher = VersionMatcher.Create(expression);

      var basicMatcher = (BasicVersionMatcher) matcher;
      Assert.Equal(operation, basicMatcher.Operation);
      Assert.Equal(
        new SemVerVersionInfo(version),
        basicMatcher.BaseVersion
      );
    }

    [Fact]
    public void HandlesNoMatchingOperationKind() {
      var basicMatcher = new BasicVersionMatcher();
      var versionInfo = new SemVerVersionInfo("1.0.0");
      Assert.False(basicMatcher.DoesMatch(versionInfo));
    }
  }
}
