using System.Linq;

namespace Corgibytes.Freshli.Lib.Languages.Perl
{
    public abstract class VersionMatcher
    {
        public static VersionMatcher Create(string value)
        {
            value = value.Trim();

            if (value.Contains(','))
            {
                var splits = value.Split(',');
                return new CompoundVersionMatcher(
                  splits.Select(Create).ToArray()
                );
            }

            return new BasicVersionMatcher
            {
                Operation = value.ToOperationKind(),
                BaseVersion = new SemVerVersionInfo(StripVersionRequirements(value))
            };
        }

        public abstract bool DoesMatch(IVersionInfo version);

        public static string StripVersionRequirements(string value)
        {
            return value.Trim().Replace("==", "")
              .Replace(">=", "")
              .Replace("<=", "")
              .Replace("<", "")
              .Replace(">", "")
              .Replace("!=", "").Trim();
        }
    }

    public class BasicVersionMatcher : VersionMatcher
    {
        public OperationKind Operation { get; set; }
        public SemVerVersionInfo BaseVersion { get; set; }

        public override bool DoesMatch(IVersionInfo version)
        {
            switch (Operation)
            {
                case OperationKind.NotEqual:
                    return version.CompareTo(BaseVersion) != 0;
                case OperationKind.GreaterThan:
                    return version.CompareTo(BaseVersion) > 0;
                case OperationKind.GreaterThanEqual:
                    return version.CompareTo(BaseVersion) >= 0;
                case OperationKind.LessThan:
                    return version.CompareTo(BaseVersion) < 0;
                case OperationKind.LessThanEqual:
                    return version.CompareTo(BaseVersion) <= 0;
                case OperationKind.Matching:
                    return version.CompareTo(BaseVersion) == 0;
                default:
                    return false;
            }
        }
    }

    public class CompoundVersionMatcher : VersionMatcher
    {
        private VersionMatcher[] _matchers;

        public CompoundVersionMatcher(VersionMatcher[] matchers)
        {
            _matchers = matchers;
        }

        public override bool DoesMatch(IVersionInfo version)
        {
            return _matchers.All(m => m.DoesMatch(version));
        }
    }
}
