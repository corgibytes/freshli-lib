using System;
using System.Collections.Generic;

namespace Freshli.Web.Models {
  public class LibYearResult {
    public Guid Id { get; set; }

    public Guid MetricsResultId { get; set; }
    public MetricsResult MetricsResult { get; set; }

    public double Total { get; set; }

    public List<LibYearPackageResult> PackageResults { get; set; }
  }
}
