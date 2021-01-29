using System;
using System.Text.RegularExpressions;

namespace Freshli.Languages.Perl {
    public enum OperationKind {
        NotFound,
        Matching,
        LessThan,
        LessThanEqual,
        GreaterThan,
        GreaterThanEqual,
        NotEqual
    }

    public static class OperationKindExtensions
    {
        public static OperationKind ToOperationKind(this string value)
        {
            string pattern = @"[<>=!]{1,2}";
            Match m = Regex.Match(value, pattern);
            var versionRequirement = m.Value;

            switch (versionRequirement)
            {
                case "==":
                    return OperationKind.Matching;
                case ">=":
                    return OperationKind.GreaterThanEqual;
                case "<=":
                    return OperationKind.LessThanEqual;
                case ">":
                    return OperationKind.GreaterThan;
                case "<":
                    return OperationKind.LessThan;
                case "!=":
                    return OperationKind.NotEqual;
                default:
                    // If no matches found, will always default to greater
                    // than or equal
                    return OperationKind.GreaterThanEqual;
            }
        }
    }
}
