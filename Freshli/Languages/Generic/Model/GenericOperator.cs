namespace Freshli.Languages.Generic.Model {
  public static class GenericOperator {
    public enum Operator {
      Equals,
      LessThan,
      LessThanOrEqualTo,
      GreaterThan,
      GreaterThanOrEqualTo,
      LatestMinor,
      Unknown
    }

    public static Operator FromStringToOperator(string op) {
      return op switch {
        "=" => Operator.Equals,
        "<" => Operator.LessThan,
        "<=" => Operator.LessThanOrEqualTo,
        ">" => Operator.GreaterThan,
        ">=" => Operator.GreaterThanOrEqualTo,
        "latest" => Operator.LatestMinor,
        _ => Operator.Unknown
      };
    }

    public static string FromOperatorToString(Operator op) {
      return op switch {
        Operator.Equals => "=",
        Operator.LessThan => "<",
        Operator.LessThanOrEqualTo => "<=",
        Operator.GreaterThan => ">",
        Operator.GreaterThanOrEqualTo => ">=",
        Operator.LatestMinor => "latest",
        _ => "?"
      };
    }
  }
}
