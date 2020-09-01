using System;

namespace Freshli {
  public class MetricsResult {
    public DateTime Date;
    public readonly LibYearResult LibYear;
    private readonly string _manifestSha;

    public MetricsResult(
      DateTime date, string manifestSha, LibYearResult libYear) {
      Date = date;
      _manifestSha = manifestSha;
      LibYear = libYear;
    }

    public override string ToString() {
      return $"{{ Date: {Date:s}, ManifestSHA: {_manifestSha}, " +
        $"LibYear: {LibYear} }}\n";
    }
  }
}
