using System;
using System.Collections.Generic;
using System.Linq;
using Corgibytes.Freshli.Lib.Extensions;

namespace Corgibytes.Freshli.Lib.Languages.Python
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
        public PythonVersionInfo BaseVersion { get; private set; }

        // TODO: Move towards factory class for creating the correct matcher
        public static VersionMatcher Create(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return new AnyVersionMatcher();
            }

            if (value.Contains(","))
            {
                return ProcessCommaVersionMatcher(value);
            }

            value = RemoveEnvironmentMarkers(value);

            switch (value.GetVersionRequirement())
            {
                case "==":
                    return ProcessEqual(ref value);
                case "<":
                case "<=":
                    return ProcessLessThan(value);
                case ">":
                case ">=":
                    return ProcessGreaterThan(value);
                case "!=":
                    return ProcessNotEqual(ref value);
                case "~=":
                    return ProcessCompatible(value);
                default:
                    throw new ArgumentException(
                      $"Invalid value '{value}' provided for Python " +
                      "VersionMatcher.Create()");
            }
        }

        private static VersionMatcher ProcessGreaterThan(string value)
        {
            return ProcessComparison(ref value, OperationKind.GreaterThan);
        }

        private static VersionMatcher ProcessLessThan(string value)
        {
            return ProcessComparison(ref value, OperationKind.LessThan);
        }

        private static VersionMatcher ProcessCompatible(string value)
        {
            var compound = new CompoundVersionMatcher();
            compound.Operation = OperationKind.Compatible;
            compound.BaseVersion =
              new PythonVersionInfo(value.Remove(0, 2));

            var first = new BasicVersionMatcher();
            first.Operation = OperationKind.GreaterThanEqual;
            var firstVersion =
              new PythonVersionInfo(compound.BaseVersion.Version);

            first.BaseVersion = firstVersion;
            compound.Add(first);

            var secondVersion =
              new PythonVersionInfo(compound.BaseVersion.Version);
            secondVersion.RemoveLastReleaseIncrement();
            secondVersion.PreRelease = null;
            secondVersion.PostRelease = null;
            secondVersion.DevelopmentRelease = null;

            var second =
              new BasicVersionMatcher
              {
                  Operation = OperationKind.PrefixMatching,
                  BaseVersion = secondVersion
              };
            compound.Add(second);
            return compound;
        }

        private static VersionMatcher ProcessNotEqual(ref string value)
        {
            VersionMatcher result = new BasicVersionMatcher();
            value = value.Remove(0, 2);
            result.Operation = OperationKind.NotEqual;

            result.BaseVersion = new PythonVersionInfo(value);
            return result;
        }

        private static VersionMatcher ProcessComparison(
          ref string value,
          OperationKind operationKind
        )
        {
            var validOperators = new List<OperationKind> {
        OperationKind.GreaterThan, OperationKind.LessThan
      };
            if (!validOperators.Contains(operationKind))
            {
                throw new ArgumentException(
                  "Invalid operation passed into ProcessComparison()"
                );
            }

            VersionMatcher result = new BasicVersionMatcher();
            value = value.Remove(0, 1);
            result.Operation = operationKind;

            if (value.StartsWith("="))
            {
                value = value.Remove(0, 1);
                result.Operation = operationKind == OperationKind.GreaterThan ?
                  OperationKind.GreaterThanEqual :
                  OperationKind.LessThanEqual;
            }

            result.BaseVersion = new PythonVersionInfo(value);
            return result;
        }

        private static VersionMatcher ProcessEqual(ref string value)
        {
            VersionMatcher result = new BasicVersionMatcher();
            value = value.Remove(0, 2);
            result.Operation = OperationKind.Matching;

            if (value.StartsWith("="))
            {
                throw new Exception(
                  $"Unsupported matcher: {OperationKind.Arbitrary}"
                );
            }

            if (value.EndsWith(".*"))
            {
                result.Operation = OperationKind.PrefixMatching;
                value = value.Replace(".*", "");
            }

            result.BaseVersion = new PythonVersionInfo(value);
            return result;
        }

        private static VersionMatcher ProcessCommaVersionMatcher(string value)
        {
            VersionMatcher result;
            var subExpressions = value.Split(",");
            var compound = new CompoundVersionMatcher();

            foreach (var subExpression in subExpressions)
            {
                compound.Add(Create(subExpression));
            }

            result = compound;
            return result;
        }

        public abstract bool DoesMatch(IVersionInfo version);

        private static string RemoveEnvironmentMarkers(string value)
        {
            var pos = value.IndexOf(";", StringComparison.Ordinal);
            return pos > 0 ? value.Remove(pos) : value;
        }
    }

    public class BasicVersionMatcher : VersionMatcher
    {
        public override bool DoesMatch(IVersionInfo version)
        {
            switch (Operation)
            {
                case OperationKind.Matching:
                    return version.CompareTo(BaseVersion) == 0;
                case OperationKind.PrefixMatching:
                    return version.Version.StartsWith(BaseVersion.Version);
                case OperationKind.LessThan:
                    return version.CompareTo(BaseVersion) < 0;
                case OperationKind.LessThanEqual:
                    return version.CompareTo(BaseVersion) <= 0;
                case OperationKind.GreaterThan:
                    return version.CompareTo(BaseVersion) > 0;
                case OperationKind.GreaterThanEqual:
                    return version.CompareTo(BaseVersion) >= 0;
                case OperationKind.NotEqual:
                    return version.CompareTo(BaseVersion) != 0;
                default:
                    throw new ArgumentException($"Invalid OperationKind {Operation} " +
                                                "provided for VersionMatcher.DoesMatch()"
                                                );
            }
        }
    }

    public class CompoundVersionMatcher : VersionMatcher
    {
        private IList<VersionMatcher> _matchers = new List<VersionMatcher>();

        public void Add(VersionMatcher matcher)
        {
            _matchers.Add(matcher);
        }

        public VersionMatcher this[int index] => _matchers[index];

        public override bool DoesMatch(IVersionInfo version)
        {
            return _matchers.All(matcher => matcher.DoesMatch(version));
        }
    }

    public class AnyVersionMatcher : VersionMatcher
    {
        public override bool DoesMatch(IVersionInfo version)
        {
            return true;
        }
    }
}
