using System;
using System.Collections.Generic;

namespace Freshli.Web.Models {
  public class AnalysisRequest {
    public enum Status {
      New,
      InProgress,
      Success,
      Invalid,
      Error,
      Retrying
    }

    public Guid Id { get; set; }
    public string Url { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }

    public Status State { get; set; }

    public virtual List<MetricsResult> Results { get; set; }
  }
}
