using System;
using System.Collections.Generic;

namespace Freshli.Web.Models {
  public class AnalysisRequest {
    public Guid Id { get; set; }
    public string Url { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }

    public List<MetricsResult> Results { get; set; }
  }
}
