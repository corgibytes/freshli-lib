using System;
using System.Collections.Generic;
using System.Linq;
using LibGit2Sharp;

namespace LibMetrics.Languages.Python
{
  public abstract class VersionMatcher
  {
    public enum OperationKind
    {
      Matching,
      PrefixMatching,
      LessThan,
      LessThanEqual,
      GreaterThan,
      GreaterThanEqual,
      NotEqual,
      Compatible,
      Arbitrary
    }

    public OperationKind Operation { get; private set; }
    public VersionInfo BaseVersion { get; private set; }

    public static VersionMatcher Create(string value)
    {
      VersionMatcher result;

      if (value.StartsWith("=="))
      {
        result = new BasicVersionMatcher();
        value = value.Remove(0, 2);
        result.Operation = OperationKind.Matching;

        if (value.StartsWith("="))
        {
          throw new Exception(
            $"Unsupported matcher: {OperationKind.Arbitrary}");
        }

        if (value.EndsWith(".*"))
        {
          result.Operation = OperationKind.PrefixMatching;
          value = value.Replace(".*", "");
        }

        result.BaseVersion = new VersionInfo() {Version = value};
      }
      else if (value.StartsWith("<"))
      {
        result = new BasicVersionMatcher();
        value = value.Remove(0, 1);
        result.Operation = OperationKind.LessThan;

        if (value.StartsWith("="))
        {
          value = value.Remove(0, 1);
          result.Operation = OperationKind.LessThanEqual;
        }

        result.BaseVersion = new VersionInfo() {Version = value};
      }
      else if (value.StartsWith(">"))
      {
        result = new BasicVersionMatcher();
        value = value.Remove(0, 1);
        result.Operation = OperationKind.GreaterThan;

        if (value.StartsWith("="))
        {
          value = value.Remove(0, 1);
          result.Operation = OperationKind.GreaterThanEqual;
        }

        result.BaseVersion = new VersionInfo() {Version = value};
      }
      else if (value.StartsWith("!="))
      {
        result = new BasicVersionMatcher();
        value = value.Remove(0, 2);
        result.Operation = OperationKind.NotEqual;

        result.BaseVersion = new VersionInfo() {Version = value};
      }
      else if (value.StartsWith("~="))
      {
        var compound = new CompoundVersionMatcher();
        compound.Operation = OperationKind.Compatible;
        compound.BaseVersion = new VersionInfo()
        {
          Version = value.Remove(0, 2)
        };

        var first = new BasicVersionMatcher();
        first.Operation = OperationKind.GreaterThanEqual;
        first.BaseVersion = compound.BaseVersion;
        compound.Add(first);

        var secondVersion = new VersionInfo()
        {
          Version = first.BaseVersion.Version
        };
        secondVersion.BuildMetadata = null;
        secondVersion.PreRelease = null;

        if (secondVersion.Minor.HasValue && secondVersion.Patch.HasValue)
        {
          secondVersion.Patch = null;
          var second = new BasicVersionMatcher();
          second.Operation = OperationKind.Matching;
          second.BaseVersion = secondVersion;
          compound.Add(second);
        }
        else if (secondVersion.Minor.HasValue)
        {
          secondVersion.Minor = null;
          var second = new BasicVersionMatcher();
          second.Operation = OperationKind.Matching;
          second.BaseVersion = secondVersion;
          compound.Add(second);
        }

        result = compound;
      }
      else
      {
        result = null;
      }

      return result;
    }

    public abstract bool DoesMatch(VersionInfo version);
  }

  public class BasicVersionMatcher : VersionMatcher
  {
    public override bool DoesMatch(VersionInfo version)
    {
      if (Operation == OperationKind.Matching)
      {
        return version.CompareTo(BaseVersion) == 0;
      }

      if (Operation == OperationKind.PrefixMatching)
      {
        return version.Version.StartsWith(BaseVersion.Version) &&
               version.CompareTo(BaseVersion) >= 0;
      }

      if (Operation == OperationKind.LessThan)
      {
        return version.CompareTo(BaseVersion) < 0;
      }

      if (Operation == OperationKind.LessThanEqual)
      {
        return version.CompareTo(BaseVersion) <= 0;
      }

      if (Operation == OperationKind.GreaterThan)
      {
        return version.CompareTo(BaseVersion) > 0;
      }

      if (Operation == OperationKind.GreaterThanEqual)
      {
        return version.CompareTo(BaseVersion) >= 0;
      }

      if (Operation == OperationKind.NotEqual)
      {
        return version.CompareTo(BaseVersion) != 0;
      }

      return false;
    }
  }

  public class CompoundVersionMatcher : VersionMatcher
  {
    private IList<VersionMatcher> _matchers = new List<VersionMatcher>();

    public void Add(VersionMatcher matcher)
    {
      _matchers.Add(matcher);
    }

    public override bool DoesMatch(VersionInfo version)
    {
      return _matchers.All(matcher => matcher.DoesMatch(version));
    }
  }
}
