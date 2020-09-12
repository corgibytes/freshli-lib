using System;

namespace Freshli.Web.Models {
  public class MetricsResult {
    public Guid Id { get; set; }

    public Guid AnalysisRequestId { get; set; }
    public virtual AnalysisRequest AnalysisRequest { get; set; }

    public DateTime Date { get; set; }
    public Guid LibYearResultId { get; set; }
    public virtual LibYearResult LibYearResult { get; set; }
  }
}
