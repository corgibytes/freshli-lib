using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Freshli.Web.Models {
  public sealed class AnalysisRequest {
    public Guid Id { get; set; }

    [Required]
    [Url]
    public string Url { get; set; }
    public string Name { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    public List<MetricsResult> Results { get; set; }

    public bool IsValid()
    {
      return !string.IsNullOrWhiteSpace(Email) &&
             !string.IsNullOrWhiteSpace(Url);
    }
  }
}
