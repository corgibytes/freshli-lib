using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Freshli.Web.Models {
  public class AnalysisRequest {
    public Guid Id { get; set; }

    [Required(ErrorMessage="Please enter a URL")]
    [Url]
    public string Url { get; set; }
    public string Name { get; set; }

    [Required(ErrorMessage="Please enter an email address")]
    [EmailAddress]
    public string Email { get; set; }

    public AnalysisRequestStatus State { get; set; }

    public virtual List<MetricsResult> Results { get; set; }
  }

  public enum AnalysisRequestStatus {
    New,
    InProgress,
    Success,
    Invalid,
    Error,
    Retrying
  }
}
