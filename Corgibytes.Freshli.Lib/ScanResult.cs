using System;
using System.Collections.Generic;

namespace Corgibytes.Freshli.Lib {
  public record ScanResult(string Filename, List<MetricsResult> MetricResults);
}
