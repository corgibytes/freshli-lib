using System;

namespace Freshli.Web.Models {
  public class MetricsResult {
    public Guid Id { get; set; }

    public Guid AnalysisRequestId { get; set; }
    public AnalysisRequest AnalysisRequest { get; set; }

    public DateTime Date { get; set; }
    public Guid LibYearResultId { get; set; }
    public LibYearResult LibYearResult { get; set; }
  }
}
