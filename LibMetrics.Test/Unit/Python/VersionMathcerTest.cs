using System;
using LibMetrics.Languages.Python;
using Xunit;

namespace LibMetrics.Test.Unit
{
  public class VersionMatcherTest
  {
    [Theory]
    [InlineData("!=1", VersionMatcher.OperationKind.NotEqual, "1")]
    [InlineData("!=1.1", VersionMatcher.OperationKind.NotEqual, "1.1")]
    [InlineData("!=1.1.1", VersionMatcher.OperationKind.NotEqual, "1.1.1")]
    [InlineData("!=1.1.1-alpha", VersionMatcher.OperationKind.NotEqual, "1.1.1-alpha")]
    [InlineData("!=1.1.1-alpha+dev", VersionMatcher.OperationKind.NotEqual, "1.1.1-alpha+dev")]
    [InlineData("<1", VersionMatcher.OperationKind.LessThan, "1")]
    [InlineData("<1.1", VersionMatcher.OperationKind.LessThan, "1.1")]
    [InlineData("<1.1.1", VersionMatcher.OperationKind.LessThan, "1.1.1")]
    [InlineData("<1.1.1-alpha", VersionMatcher.OperationKind.LessThan, "1.1.1-alpha")]
    [InlineData("<1.1.1-alpha+dev", VersionMatcher.OperationKind.LessThan, "1.1.1-alpha+dev")]
    [InlineData("<=1", VersionMatcher.OperationKind.LessThanEqual, "1")]
    [InlineData("<=1.1", VersionMatcher.OperationKind.LessThanEqual, "1.1")]
    [InlineData("<=1.1.1", VersionMatcher.OperationKind.LessThanEqual, "1.1.1")]
    [InlineData("<=1.1.1-alpha", VersionMatcher.OperationKind.LessThanEqual, "1.1.1-alpha")]
    [InlineData("<=1.1.1-alpha+dev", VersionMatcher.OperationKind.LessThanEqual, "1.1.1-alpha+dev")]
    [InlineData("==1.*", VersionMatcher.OperationKind.PrefixMatching, "1")]
    [InlineData("==1.0.0", VersionMatcher.OperationKind.Matching, "1.0.0")]
    [InlineData("==1.1.*", VersionMatcher.OperationKind.PrefixMatching, "1.1")]
    [InlineData("==1.1.1.*", VersionMatcher.OperationKind.PrefixMatching, "1.1.1")]
    [InlineData(">1", VersionMatcher.OperationKind.GreaterThan, "1")]
    [InlineData(">1.1", VersionMatcher.OperationKind.GreaterThan, "1.1")]
    [InlineData(">1.1.1", VersionMatcher.OperationKind.GreaterThan, "1.1.1")]
    [InlineData(">1.1.1-alpha", VersionMatcher.OperationKind.GreaterThan, "1.1.1-alpha")]
    [InlineData(">1.1.1-alpha+dev", VersionMatcher.OperationKind.GreaterThan, "1.1.1-alpha+dev")]
    [InlineData(">=1", VersionMatcher.OperationKind.GreaterThanEqual, "1")]
    [InlineData(">=1.1", VersionMatcher.OperationKind.GreaterThanEqual, "1.1")]
    [InlineData(">=1.1.1", VersionMatcher.OperationKind.GreaterThanEqual, "1.1.1")]
    [InlineData(">=1.1.1-alpha", VersionMatcher.OperationKind.GreaterThanEqual, "1.1.1-alpha")]
    [InlineData(">=1.1.1-alpha+dev", VersionMatcher.OperationKind.GreaterThanEqual, "1.1.1-alpha+dev")]
    public void SplitsIntoComponents(
      string expression,
      VersionMatcher.OperationKind operationKind,
      string baseVersion)
    {
      var matcher = VersionMatcher.Create(expression);
      Assert.Equal(operationKind, matcher.Operation);
      Assert.Equal(new VersionInfo {Version = baseVersion}, matcher.BaseVersion);
    }

    [Theory]
    [InlineData(
      "~=1",
      VersionMatcher.OperationKind.Compatible,
      "1",
      VersionMatcher.OperationKind.GreaterThanEqual,
      "1",
      null,
      null)]
    [InlineData(
      "~=1.1",
      VersionMatcher.OperationKind.Compatible,
      "1.1",
      VersionMatcher.OperationKind.GreaterThanEqual,
      "1.1",
      VersionMatcher.OperationKind.Matching,
      "1")]
    [InlineData(
      "~=1.1.1",
      VersionMatcher.OperationKind.Compatible,
      "1.1.1",
      VersionMatcher.OperationKind.GreaterThanEqual,
      "1.1.1",
      VersionMatcher.OperationKind.Matching,
      "1.1")]
    [InlineData(
      "~=1.1.1-alpha",
      VersionMatcher.OperationKind.Compatible,
      "1.1.1-alpha",
      VersionMatcher.OperationKind.GreaterThanEqual,
      "1.1.1",
      VersionMatcher.OperationKind.Matching,
      "1.1")]
    [InlineData(
      "~=1.1.1-alpha+dev",
      VersionMatcher.OperationKind.Compatible,
      "1.1.1-alpha+dev",
      VersionMatcher.OperationKind.GreaterThanEqual,
      "1.1.1",
      VersionMatcher.OperationKind.Matching,
      "1.1")]
    public void CorrectlyCreatesCompoundMatchers(
      string expression,
      VersionMatcher.OperationKind operationKind,
      string baseVersion,
      VersionMatcher.OperationKind firstChildOperation,
      string firstChildBaseVersion,
      VersionMatcher.OperationKind? secondChildOperation,
      string secondChildBaseVersion
    )
    {
      var matcher = VersionMatcher.Create(expression);
      Assert.Equal(operationKind, matcher.Operation);
      Assert.Equal(new VersionInfo {Version = baseVersion}, matcher.BaseVersion);

      var compoundMatcher = (CompoundVersionMatcher) matcher;

      var firstChild = compoundMatcher[0];
      Assert.Equal(firstChildOperation, firstChild.Operation);
      Assert.Equal(new VersionInfo {Version = firstChildBaseVersion}, firstChild.BaseVersion);

      if (secondChildOperation.HasValue)
      {
        var secondChild = compoundMatcher[1];
        Assert.Equal(secondChildOperation, secondChild.Operation);
        Assert.Equal(new VersionInfo {Version = secondChildBaseVersion}, secondChild.BaseVersion);
      }
    }

    [Theory]
    [InlineData("0.0.1", "!=1.0.0", true)]
    [InlineData("0.0.1", "<1.0.0", true)]
    [InlineData("0.0.1", "<=1.0.0", true)]
    [InlineData("0.0.1", ">1.0.0", false)]
    [InlineData("0.0.1", ">=1.0.0", false)]
    [InlineData("0.0.1", "~=1.0.0", false)]
    [InlineData("0.1.0", "!=1.0.0", true)]
    [InlineData("0.1.0", "<1.0.0", true)]
    [InlineData("0.1.0", "<=1.0.0", true)]
    [InlineData("0.1.0", ">1.0.0", false)]
    [InlineData("0.1.0", ">=1.0.0", false)]
    [InlineData("0.1.0", "~=1.0.0", false)]
    [InlineData("1.0.0", "!=0.0.1", true)]
    [InlineData("1.0.0", "!=0.1.0", true)]
    [InlineData("1.0.0", "!=1.0.0", false)]
    [InlineData("1.0.0", "!=1.0.0-alpha", true)]
    [InlineData("1.0.0", "!=1.0.1", true)]
    [InlineData("1.0.0", "!=1.1.0", true)]
    [InlineData("1.0.0", "!=2.0.0", true)]
    [InlineData("1.0.0", "<0.0.1", false)]
    [InlineData("1.0.0", "<0.1.0", false)]
    [InlineData("1.0.0", "<1.0.0", false)]
    [InlineData("1.0.0", "<1.0.0-alpha", false)]
    [InlineData("1.0.0", "<1.0.1", true)]
    [InlineData("1.0.0", "<1.1.0", true)]
    [InlineData("1.0.0", "<2.0.0", true)]
    [InlineData("1.0.0", "<=0.0.1", false)]
    [InlineData("1.0.0", "<=0.1.0", false)]
    [InlineData("1.0.0", "<=1.0.0", true)]
    [InlineData("1.0.0", "<=1.0.0-alpha", false)]
    [InlineData("1.0.0", "<=1.0.1", true)]
    [InlineData("1.0.0", "<=1.1.0", true)]
    [InlineData("1.0.0", "<=2.0.0", true)]
    [InlineData("1.0.0", "==1.0.*", true)]
    [InlineData("1.0.0", "==1.0.0", true)]
    [InlineData("1.0.0", ">0.0.1", true)]
    [InlineData("1.0.0", ">0.1.0", true)]
    [InlineData("1.0.0", ">1.0.0", false)]
    [InlineData("1.0.0", ">1.0.0-alpha", true)]
    [InlineData("1.0.0", ">1.0.1", false)]
    [InlineData("1.0.0", ">1.1.0", false)]
    [InlineData("1.0.0", ">2.0.0", false)]
    [InlineData("1.0.0", ">=0.0.1", true)]
    [InlineData("1.0.0", ">=0.1.0", true)]
    [InlineData("1.0.0", ">=1.0.0", true)]
    [InlineData("1.0.0", ">=1.0.0-alpha", true)]
    [InlineData("1.0.0", ">=1.0.1", false)]
    [InlineData("1.0.0", ">=1.1.0", false)]
    [InlineData("1.0.0", ">=2.0.0", false)]
    [InlineData("1.0.0", "~=0.0.1", false)]
    [InlineData("1.0.0", "~=0.1.0", false)]
    [InlineData("1.0.0", "~=1", true)]
    [InlineData("1.0.0", "~=1.0.0", true)]
    [InlineData("1.0.0", "~=1.0.0-alpha", true)]
    [InlineData("1.0.0", "~=1.0.1", false)]
    [InlineData("1.0.0", "~=1.0.1-alpha1", false)]
    [InlineData("1.0.0", "~=1.1", false)]
    [InlineData("1.0.0", "~=1.1.0", false)]
    [InlineData("1.0.0", "~=1.1.0-alpha1", false)]
    [InlineData("1.0.0", "~=2", false)]
    [InlineData("1.0.0", "~=2.0", false)]
    [InlineData("1.0.0", "~=2.0.0", false)]
    [InlineData("1.0.0-alpha", "!=1.0.0", true)]
    [InlineData("1.0.0-alpha", "<1.0.0", true)]
    [InlineData("1.0.0-alpha", "<=1.0.0", true)]
    [InlineData("1.0.0-alpha", ">1.0.0", false)]
    [InlineData("1.0.0-alpha", ">=1.0.0", false)]
    [InlineData("1.0.0-alpha", "~=1.0.0", false)]
    [InlineData("1.0.0-alpha+test1", "!=1.0.0-alpha+test2", false)]
    [InlineData("1.0.0-alpha+test1", "<1.0.0-alpha+test2", false)]
    [InlineData("1.0.0-alpha+test1", "<=1.0.0-alpha+test2", true)]
    [InlineData("1.0.0-alpha+test1", ">1.0.0-alpha+test2", false)]
    [InlineData("1.0.0-alpha+test1", ">=1.0.0-alpha+test2", true)]
    [InlineData("1.0.0-alpha+test1", "~=1.0.0-alpha+test2", true)]
    [InlineData("1.0.0-alpha+test2", "!=1.0.0-alpha+test1", false)]
    [InlineData("1.0.0-alpha+test2", "<1.0.0-alpha+test1", false)]
    [InlineData("1.0.0-alpha+test2", "<=1.0.0-alpha+test1", true)]
    [InlineData("1.0.0-alpha+test2", ">1.0.0-alpha+test1", false)]
    [InlineData("1.0.0-alpha+test2", ">=1.0.0-alpha+test1", true)]
    [InlineData("1.0.0-alpha+test2", "~=1.0.0-alpha+test1", true)]
    [InlineData("1.0.0-alpha1", "~=1.0.1-alpha1", false)]
    [InlineData("1.0.0-alpha1", "~=1.0.1-alpha2", false)]
    [InlineData("1.0.0-alpha1", "~=1.1.0-alpha1", false)]
    [InlineData("1.0.0-alpha1", "~=1.1.0-alpha2", false)]
    [InlineData("1.0.0-alpha2", "~=1.0.1-alpha1", false)]
    [InlineData("1.0.0-alpha2", "~=1.1.0-alpha1", false)]
    [InlineData("1.0.1", "!=1.0.0", true)]
    [InlineData("1.0.1", "<1.0.0", false)]
    [InlineData("1.0.1", "<=1.0.0", false)]
    [InlineData("1.0.1", ">1.0.0", true)]
    [InlineData("1.0.1", ">=1.0.0", true)]
    [InlineData("1.0.1", "~=1.0.0", true)]
    [InlineData("1.0.1", "~=1.0.0-alpha1", false)]
    [InlineData("1.0.1-alpha1", "~=1.0.0-alpha1", false)]
    [InlineData("1.0.1-alpha1", "~=1.0.0-alpha2", false)]
    [InlineData("1.0.1-alpha2", "~=1.0.0-alpha1", false)]
    [InlineData("1.1.0", "!=1.0.0", true)]
    [InlineData("1.1.0", "<1.0.0", false)]
    [InlineData("1.1.0", "<=1.0.0", false)]
    [InlineData("1.1.0", ">1.0.0", true)]
    [InlineData("1.1.0", ">=1.0.0", true)]
    [InlineData("1.1.0", "~=1", true)]
    [InlineData("1.1.0", "~=1.0.0", false)]
    [InlineData("1.1.0", "~=1.0.0-alpha1", false)]
    [InlineData("1.1.0", "~=1.1", true)]
    [InlineData("1.1.0", "~=1.1.0", true)]
    [InlineData("1.1.0", "~=1.1.0-alpha1", true)]
    [InlineData("1.1.0-alpha1", "~=1.0.0-alpha1", false)]
    [InlineData("1.1.0-alpha1", "~=1.0.0-alpha2", false)]
    [InlineData("1.1.0-alpha1", "~=1.1.0-alpha1", true)]
    [InlineData("1.1.0-alpha1", "~=1.1.0-alpha2", false)]
    [InlineData("1.1.0-alpha2", "~=1.0.0-alpha1", false)]
    [InlineData("1.1.0-alpha2", "~=1.1.0-alpha1", true)]
    [InlineData("1.1.1", "~=1.1.0", true)]
    [InlineData("1.1.1", "~=1.1.0-alpha1", false)]
    [InlineData("1.1.1", "~=1.2.0", false)]
    [InlineData("1.1.1", "~=1.2.0-alpha1", false)]
    [InlineData("1.1.1-alpha1", "~=1.1.0-alpha1", false)]
    [InlineData("1.1.1-alpha1", "~=1.1.0-alpha2", false)]
    [InlineData("1.1.1-alpha1", "~=1.2.0-alpha1", false)]
    [InlineData("1.1.1-alpha1", "~=1.2.0-alpha2", false)]
    [InlineData("1.1.1-alpha2", "~=1.1.0-alpha1", false)]
    [InlineData("1.1.1-alpha2", "~=1.2.0-alpha1", false)]
    [InlineData("1.2.0", "~=1", true)]
    [InlineData("1.2.0", "~=1.1", true)]
    [InlineData("1.2.3", "==1.*", true)]
    [InlineData("1.2.3", "==1.0.*", false)]
    [InlineData("1.2.3", "==1.0.0", false)]
    [InlineData("1.2.3", "==1.2.*", true)]
    [InlineData("1.2.3", "==1.2.3.*", true)]
    [InlineData("2.0.0", "!=1.0.0", true)]
    [InlineData("2.0.0", "<1.0.0", false)]
    [InlineData("2.0.0", "<=1.0.0", false)]
    [InlineData("2.0.0", ">1.0.0", true)]
    [InlineData("2.0.0", ">=1.0.0", true)]
    [InlineData("2.0.0", "~=1", true)]
    [InlineData("2.0.0", "~=1.0", false)]
    [InlineData("2.0.0", "~=1.0.0", false)]
    public void DoesMatchHandlesEqualsOperator(
      string version,
      string expression,
      bool doesMatch)
    {
      var info = new VersionInfo() {Version = version};
      var matcher = VersionMatcher.Create(expression);
      Assert.Equal(doesMatch, matcher.DoesMatch(info));
    }
  }
}
