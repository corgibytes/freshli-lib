using System;

namespace Corgibytes.Freshli.Lib
{
    // TODO: Explore converting this `class` into a `record`
    public class MetricsResult
    {
        public DateTimeOffset Date;
        public readonly LibYearResult LibYear;
        private readonly string _manifestSha;

        public MetricsResult(
          DateTimeOffset date, string manifestSha, LibYearResult libYear)
        {
            Date = date;
            _manifestSha = manifestSha;
            LibYear = libYear;
        }

        public override string ToString()
        {
            return $"{{ Date: {Date:O}, ManifestSHA: {_manifestSha}, " +
              $"LibYear: {LibYear} }}{Environment.NewLine}";
        }
    }
}
