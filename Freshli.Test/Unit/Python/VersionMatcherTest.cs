using System;
using ApprovalUtilities.Utilities;
using Freshli.Languages.Python;
using Xunit;
using Xunit.Abstractions;

namespace Freshli.Test.Unit.Python {
  public class VersionMatcherTest {
    [Theory]
    [InlineData("!=1", VersionMatcher.OperationKind.NotEqual, "1")]
    [InlineData("!=1.1", VersionMatcher.OperationKind.NotEqual, "1.1")]
    [InlineData("!=1.1.1", VersionMatcher.OperationKind.NotEqual, "1.1.1")]
    [InlineData(
      "!=1.1.1-alpha",
      VersionMatcher.OperationKind.NotEqual,
      "1.1.1-alpha"
    )]
    [InlineData(
      "!=1.1.1-alpha+dev",
      VersionMatcher.OperationKind.NotEqual,
      "1.1.1-alpha+dev"
    )]
    [InlineData("<1", VersionMatcher.OperationKind.LessThan, "1")]
    [InlineData("<1.1", VersionMatcher.OperationKind.LessThan, "1.1")]
    [InlineData("<1.1.1", VersionMatcher.OperationKind.LessThan, "1.1.1")]
    [InlineData(
      "<1.1.1-alpha",
      VersionMatcher.OperationKind.LessThan,
      "1.1.1-alpha"
    )]
    [InlineData(
      "<1.1.1-alpha+dev",
      VersionMatcher.OperationKind.LessThan,
      "1.1.1-alpha+dev"
    )]
    [InlineData("<=1", VersionMatcher.OperationKind.LessThanEqual, "1")]
    [InlineData("<=1.1", VersionMatcher.OperationKind.LessThanEqual, "1.1")]
    [InlineData("<=1.1.1", VersionMatcher.OperationKind.LessThanEqual, "1.1.1")]
    [InlineData(
      "<=1.1.1-alpha",
      VersionMatcher.OperationKind.LessThanEqual,
      "1.1.1-alpha"
    )]
    [InlineData(
      "<=1.1.1-alpha+dev",
      VersionMatcher.OperationKind.LessThanEqual,
      "1.1.1-alpha+dev"
    )]
    [InlineData("==1.*", VersionMatcher.OperationKind.PrefixMatching, "1")]
    [InlineData("==1.0.0", VersionMatcher.OperationKind.Matching, "1.0.0")]
    [InlineData("==1.1.*", VersionMatcher.OperationKind.PrefixMatching, "1.1")]
    [InlineData(
      "==1.1.1.*",
      VersionMatcher.OperationKind.PrefixMatching,
      "1.1.1"
    )]
    [InlineData(">1", VersionMatcher.OperationKind.GreaterThan, "1")]
    [InlineData(">1.1", VersionMatcher.OperationKind.GreaterThan, "1.1")]
    [InlineData(">1.1.1", VersionMatcher.OperationKind.GreaterThan, "1.1.1")]
    [InlineData(
      ">1.1.1-alpha",
      VersionMatcher.OperationKind.GreaterThan,
      "1.1.1-alpha"
    )]
    [InlineData(
      ">1.1.1-alpha+dev",
      VersionMatcher.OperationKind.GreaterThan,
      "1.1.1-alpha+dev"
    )]
    [InlineData(">=1", VersionMatcher.OperationKind.GreaterThanEqual, "1")]
    [InlineData(">=1.1", VersionMatcher.OperationKind.GreaterThanEqual, "1.1")]
    [InlineData(
      ">=1.1.1",
      VersionMatcher.OperationKind.GreaterThanEqual,
      "1.1.1"
    )]
    [InlineData(
      ">=1.1.1-alpha",
      VersionMatcher.OperationKind.GreaterThanEqual,
      "1.1.1-alpha"
    )]
    [InlineData(
      ">=1.1.1-alpha+dev",
      VersionMatcher.OperationKind.GreaterThanEqual,
      "1.1.1-alpha+dev"
    )]
    public void SplitsIntoComponents(
      string expression,
      VersionMatcher.OperationKind operationKind,
      string baseVersion
    ) {
      var matcher = VersionMatcher.Create(expression);
      Assert.Equal(operationKind, matcher.Operation);
      Assert.Equal(
        new VersionInfo {Version = baseVersion},
        matcher.BaseVersion
      );
    }

    [Theory]
    [InlineData(
      "~=1",
      VersionMatcher.OperationKind.Compatible,
      "1",
      VersionMatcher.OperationKind.GreaterThanEqual,
      "1",
      null,
      null
    )]
    [InlineData(
      "~=1.1",
      VersionMatcher.OperationKind.Compatible,
      "1.1",
      VersionMatcher.OperationKind.GreaterThanEqual,
      "1.1",
      VersionMatcher.OperationKind.PrefixMatching,
      "1"
    )]
    [InlineData(
      "~=1.1.1",
      VersionMatcher.OperationKind.Compatible,
      "1.1.1",
      VersionMatcher.OperationKind.GreaterThanEqual,
      "1.1.1",
      VersionMatcher.OperationKind.PrefixMatching,
      "1.1"
    )]
    [InlineData(
      "~=1.1.1-alpha",
      VersionMatcher.OperationKind.Compatible,
      "1.1.1-alpha",
      VersionMatcher.OperationKind.GreaterThanEqual,
      "1.1.1-alpha",
      VersionMatcher.OperationKind.PrefixMatching,
      "1.1"
    )]
    [InlineData(
      "~=1.1.1-alpha+dev",
      VersionMatcher.OperationKind.Compatible,
      "1.1.1-alpha+dev",
      VersionMatcher.OperationKind.GreaterThanEqual,
      "1.1.1-alpha+dev",
      VersionMatcher.OperationKind.PrefixMatching,
      "1.1"
    )]
    public void CorrectlyCreatesCompatibleOperatorsAsCompoundMatchers(
      string expression,
      VersionMatcher.OperationKind operationKind,
      string baseVersion,
      VersionMatcher.OperationKind firstChildOperation,
      string firstChildBaseVersion,
      VersionMatcher.OperationKind? secondChildOperation,
      string secondChildBaseVersion
    ) {
      var matcher = VersionMatcher.Create(expression);
      Assert.Equal(operationKind, matcher.Operation);
      Assert.Equal(
        new VersionInfo {Version = baseVersion},
        matcher.BaseVersion
      );

      var compoundMatcher = (CompoundVersionMatcher) matcher;

      var firstChild = compoundMatcher[0];
      Assert.Equal(firstChildOperation, firstChild.Operation);

      var expectedFirstChildVersion =
        new VersionInfo {Version = firstChildBaseVersion};
      Assert.Equal(expectedFirstChildVersion, firstChild.BaseVersion);

      if (secondChildOperation.HasValue) {
        var secondChild = compoundMatcher[1];
        Assert.Equal(secondChildOperation, secondChild.Operation);

        var expectedSecondChildVersion =
          new VersionInfo {Version = secondChildBaseVersion};
        Assert.Equal(expectedSecondChildVersion, secondChild.BaseVersion);
      }
    }

    [Theory]
    [InlineData(
      ">=2017.2,<2020.1",
      VersionMatcher.OperationKind.GreaterThanEqual,
      "2017.2",
      VersionMatcher.OperationKind.LessThan,
      "2020.1"
    )]
    public void CorrectlyCreatesCommaSeparatedCompoundMatchers(
      string expression,
      VersionMatcher.OperationKind firstChildOperation,
      string firstChildBaseVersion,
      VersionMatcher.OperationKind? secondChildOperation,
      string secondChildBaseVersion
    ) {
      var matcher = VersionMatcher.Create(expression);

      var compoundMatcher = (CompoundVersionMatcher) matcher;

      var firstChild = compoundMatcher[0];
      Assert.Equal(firstChildOperation, firstChild.Operation);

      var expectedFirstChildVersion =
        new VersionInfo {Version = firstChildBaseVersion};
      Assert.Equal(expectedFirstChildVersion, firstChild.BaseVersion);

      if (secondChildOperation.HasValue) {
        var secondChild = compoundMatcher[1];
        Assert.Equal(secondChildOperation, secondChild.Operation);

        var expectedSecondChildVersion =
          new VersionInfo {Version = secondChildBaseVersion};
        Assert.Equal(expectedSecondChildVersion, secondChild.BaseVersion);
      }
    }

    [Fact]
    public void CorrectlyCreatesAnAnyMatcherWhenGivenAnEmptyString() {
      var matcher = VersionMatcher.Create("");

      Assert.True(matcher.DoesMatch(new VersionInfo {Version = "anything"}));
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
    [InlineData("1.0.0", ">=1", true)]
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
    [InlineData("1.0.0-alpha+test1", "==1.0.*", true)]
    [InlineData("1.0.0-alpha+test1", ">1.0.0-alpha+test2", false)]
    [InlineData("1.0.0-alpha+test1", ">=1.0.0-alpha+test2", true)]
    [InlineData("1.0.0-alpha+test1", "~=1.0.0-alpha+test2", true)]
    [InlineData("1.0.0-alpha+test2", "!=1.0.0-alpha+test1", false)]
    [InlineData("1.0.0-alpha+test2", "<1.0.0-alpha+test1", false)]
    [InlineData("1.0.0-alpha+test2", "<=1.0.0-alpha+test1", true)]
    [InlineData("1.0.0-alpha+test2", "==1.0.*", true)]
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
    [InlineData("1.0.1", "==1.0.*", true)]
    [InlineData("1.0.1", ">1.0.0", true)]
    [InlineData("1.0.1", ">=1.0.0", true)]
    [InlineData("1.0.1", "~=1.0.0", true)]
    [InlineData("1.0.1", "~=1.0.0-alpha1", true)]
    [InlineData("1.0.1-alpha1", "~=1.0.0-alpha1", true)]
    [InlineData("1.0.1-alpha1", "~=1.0.0-alpha2", true)]
    [InlineData("1.0.1-alpha2", "~=1.0.0-alpha1", true)]
    [InlineData("1.1.0", "!=1.0.0", true)]
    [InlineData("1.1.0", "<1.0.0", false)]
    [InlineData("1.1.0", "<=1.0.0", false)]
    [InlineData("1.1.0", "==1.*", true)]
    [InlineData("1.1.0", "==1.1.*", true)]
    [InlineData("1.1.0", ">1.0.0", true)]
    [InlineData("1.1.0", ">=1", true)]
    [InlineData("1.1.0", ">=1.0.0", true)]
    [InlineData("1.1.0", ">=1.1", true)]
    [InlineData("1.1.0", ">=1.1.0", true)]
    [InlineData("1.1.0", ">=1.1.0-alpha1", true)]
    [InlineData("1.1.0", "~=1", true)]
    [InlineData("1.1.0", "~=1.0.0", false)]
    [InlineData("1.1.0", "~=1.0.0-alpha1", false)]
    [InlineData("1.1.0", "~=1.1", true)]
    [InlineData("1.1.0", "~=1.1.0", true)]
    [InlineData("1.1.0", "~=1.1.0-alpha1", true)]
    [InlineData("1.1.0-alpha1", "==1.1.*", true)]
    [InlineData("1.1.0-alpha1", ">=1.1.0-alpha1", true)]
    [InlineData("1.1.0-alpha1", "~=1.0.0-alpha1", false)]
    [InlineData("1.1.0-alpha1", "~=1.0.0-alpha2", false)]
    [InlineData("1.1.0-alpha1", "~=1.1.0-alpha1", true)]
    [InlineData("1.1.0-alpha1", "~=1.1.0-alpha2", false)]
    [InlineData("1.1.0-alpha2", "==1.1.*", true)]
    [InlineData("1.1.0-alpha2", ">=1.1.0-alpha1", true)]
    [InlineData("1.1.0-alpha2", "~=1.0.0-alpha1", false)]
    [InlineData("1.1.0-alpha2", "~=1.1.0-alpha1", true)]
    [InlineData("1.1.1", "==1.1.*", true)]
    [InlineData("1.1.1", ">=1.1.0", true)]
    [InlineData("1.1.1", "~=1.1.0", true)]
    [InlineData("1.1.1", "~=1.1.0-alpha1", true)]
    [InlineData("1.1.1", "~=1.2.0", false)]
    [InlineData("1.1.1", "~=1.2.0-alpha1", false)]
    [InlineData("1.1.1-alpha1", "~=1.1.0-alpha1", true)]
    [InlineData("1.1.1-alpha1", "~=1.1.0-alpha2", true)]
    [InlineData("1.1.1-alpha1", "~=1.2.0-alpha1", false)]
    [InlineData("1.1.1-alpha1", "~=1.2.0-alpha2", false)]
    [InlineData("1.1.1-alpha2", "~=1.1.0-alpha1", true)]
    [InlineData("1.1.1-alpha2", "~=1.2.0-alpha1", false)]
    [InlineData("1.2.0", "==1.*", true)]
    [InlineData("1.2.0", ">=1", true)]
    [InlineData("1.2.0", ">=1.1", true)]
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
    [InlineData("2.0.0", ">=1", true)]
    [InlineData("2.0.0", ">=1.0.0", true)]
    [InlineData("2.0.0", "~=1", true)]
    [InlineData("2.0.0", "~=1.0", false)]
    [InlineData("2.0.0", "~=1.0.0", false)]
    [InlineData("2.2.0", "==2.2", true)]
    [InlineData("2.2.1", "==2.2", false)]
    public void DoesMatchHandleVersionSpecifierOperator(
      string version,
      string expression,
      bool doesMatch
    ) {
      var info = new VersionInfo() {Version = version};
      var matcher = VersionMatcher.Create(expression);
      Assert.Equal(doesMatch, matcher.DoesMatch(info));
    }
  }
}
