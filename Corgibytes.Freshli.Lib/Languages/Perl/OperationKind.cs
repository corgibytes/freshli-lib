using Corgibytes.Freshli.Lib.Extensions;

namespace Corgibytes.Freshli.Lib.Languages.Perl {
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
            switch (value.GetVersionRequirement())
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
