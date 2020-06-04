using System;
using LibMetrics.Languages.Python;
using Xunit;

namespace LibMetrics.Test.Unit
{
  public class VersionMatcherTest
  {
    [Theory]
    [InlineData("==1.0.0", VersionMatcher.OperationKind.Matching, "1.0.0")]
    [InlineData("==1.*", VersionMatcher.OperationKind.PrefixMatching, "1")]
    [InlineData("==1.1.*", VersionMatcher.OperationKind.PrefixMatching, "1.1")]
    [InlineData("==1.1.1.*", VersionMatcher.OperationKind.PrefixMatching, "1.1.1")]
    public void SplitsIntoComponents(
      string expression,
      VersionMatcher.OperationKind operationKind,
      string baseVersion)
    {
      var matcher = new VersionMatcher(expression);
      Assert.Equal(operationKind, matcher.Operation);
      Assert.Equal(new VersionInfo {Version = baseVersion}, matcher.BaseVersion);
    }

    [Theory]
    [InlineData("1.0.0", "==1.0.0", true)]
    [InlineData("1.2.3", "==1.0.0", false)]
    [InlineData("1.2.3", "==1.0.*", false)]
    [InlineData("1.2.3", "==1.*", true)]
    [InlineData("1.2.3", "==1.2.*", true)]
    [InlineData("1.2.3", "==1.2.3.*", true)]
    public void DoesMatchHandlesEqualsOperator(
      string version,
      string expression,
      bool doesMatch)
    {
      var info = new VersionInfo() {Version = version};
      var matcher = new VersionMatcher(expression);
      Assert.Equal(doesMatch, matcher.DoesMatch(info));
    }
  }
}
