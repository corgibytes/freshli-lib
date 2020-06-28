using System.Linq;

namespace Freshli.Languages.Perl {
  public abstract class VersionMatcher {
    public enum OperationKind
    {
      Matching,
      LessThan,
      LessThanEqual,
      GreaterThan,
      GreaterThanEqual,
      NotEqual
    }
    
    public static VersionMatcher Create(string value) {
      value = value.Trim();
      
      if (value.Contains(',')) {
        var splits = value.Split(',');
        return new CompoundVersionMatcher(
          splits.Select(Create).ToArray()
        );
      }
      
      if (value.StartsWith("==")) {
        var version = value.Replace("==", "").Trim();
        return new BasicVersionMatcher {
          Operation = OperationKind.Matching,
          BaseVersion = new VersionInfo {Version = version}
        };
      }
      
      if (value.StartsWith(">=")) {
        var version = value.Replace(">=", "").Trim();
        return new BasicVersionMatcher {
          Operation = OperationKind.GreaterThanEqual,
          BaseVersion = new VersionInfo {Version = version}
        };
      }

      if (value.StartsWith("<=")) {
        var version = value.Replace("<=", "").Trim();
        return new BasicVersionMatcher {
          Operation = OperationKind.LessThanEqual,
          BaseVersion = new VersionInfo {Version = version}
        };
      }

      if (value.StartsWith("<")) {
        var version = value.Replace("<", "").Trim();
        return new BasicVersionMatcher {
          Operation = OperationKind.LessThan,
          BaseVersion = new VersionInfo {Version = version}
        };
      }

      if (value.StartsWith(">")) {
        var version = value.Replace(">", "").Trim();
        return new BasicVersionMatcher {
          Operation = OperationKind.GreaterThan,
          BaseVersion = new VersionInfo {Version = version}
        };
      }

      if (value.StartsWith("!=")) {
        var version = value.Replace("!=", "").Trim();
        return new BasicVersionMatcher {
          Operation = OperationKind.NotEqual,
          BaseVersion = new VersionInfo {Version = version}
        };
      }

      return new BasicVersionMatcher {
        Operation = OperationKind.GreaterThanEqual,
        BaseVersion = new VersionInfo {Version = value}
      };
    }

    public abstract bool DoesMatch(VersionInfo version);
  }

  public class BasicVersionMatcher : VersionMatcher {
    public OperationKind Operation { get; set; }
    public VersionInfo BaseVersion { get; set; }
    
    public override bool DoesMatch(VersionInfo version) {
      if (Operation == OperationKind.NotEqual) {
        return version.CompareTo(BaseVersion) != 0;
      }

      if (Operation == OperationKind.GreaterThan) {
        return version.CompareTo(BaseVersion) > 0;
      }
      
      if (Operation == OperationKind.GreaterThanEqual) {
        return version.CompareTo(BaseVersion) >= 0;
      }
      
      if (Operation == OperationKind.LessThan) {
        return version.CompareTo(BaseVersion) < 0;
      }
      
      if (Operation == OperationKind.LessThanEqual) {
        return version.CompareTo(BaseVersion) <= 0;
      }

      if (Operation == OperationKind.Matching) {
        return version.CompareTo(BaseVersion) == 0;
      }

      return false;
    }
  }
  
  public class CompoundVersionMatcher: VersionMatcher {
    private VersionMatcher[] _matchers;
      
    public CompoundVersionMatcher(VersionMatcher[] matchers) {
      _matchers = matchers;
    }

    public override bool DoesMatch(VersionInfo version) {
      return _matchers.All(m => m.DoesMatch(version));
    }
  }
}
