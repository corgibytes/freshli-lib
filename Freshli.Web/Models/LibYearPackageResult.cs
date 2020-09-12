using System;

namespace Freshli.Web.Models {
  public class LibYearPackageResult {
    public Guid Id { get; set; }

    public string Name { get; set; }
    public string Version { get; set; }
    public DateTime PublishedAt { get; set; }
    public double Value { get; set; }

    public Guid LibYearResultId { get; set; }
    public virtual LibYearResult LibYearResult { get; set; }
  }
}
