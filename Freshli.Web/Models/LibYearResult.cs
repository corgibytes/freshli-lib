using System;
using System.Collections.Generic;

namespace Freshli.Web.Models {
  public class LibYearResult {
    public Guid Id { get; set; }

    public Guid MetricsResultId { get; set; }
    public virtual MetricsResult MetricsResult { get; set; }

    public double Total { get; set; }

    public virtual List<LibYearPackageResult> PackageResults { get; set; }
  }
}
