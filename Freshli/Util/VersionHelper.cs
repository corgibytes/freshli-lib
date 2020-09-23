namespace Freshli.Util {
  public static class VersionHelper {

    public static int CompareNumericValues(long? v1, long? v2) {
      if (v1 == v2) {
        return 0;
      }
      if (v1 > v2) {
        return 1;
      }
      if (v1 != null && v2 == null) {
        return 1;
      }
      return -1;
    }

  }
}
