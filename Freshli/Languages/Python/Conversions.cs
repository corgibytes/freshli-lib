using System;
using System.Linq;

namespace Freshli.Languages.Python {
  public class Conversions {
    public static long? SafeConvertToInt64(string value, long? defaultValue) {
      var result = defaultValue;
      if (!string.IsNullOrWhiteSpace(value)) {
        result = Convert.ToInt64(value);
      }

      return result;
    }

    public static bool HasValue(long? value) {
      return value != null;
    }

    public static string SafeToLower(string value) {
      if (!string.IsNullOrWhiteSpace(value)) {
        return value.ToLower();
      }

      return null;
    }

    public static long[] SafeSplitIntoLongs(string value, char separator) {
      if (!string.IsNullOrWhiteSpace(value)) {
        return value.Split(separator).Select(p => Convert.ToInt64((string?) p)).ToArray();
      }
      return new long[] {};
    }

    public static string SafeExtractString(string value) {
      if (!string.IsNullOrWhiteSpace(value)) {
        return value;
      }

      return null;
    }
  }
}
