using System.Collections.Generic;
using XPlot.Plotly;

namespace Freshli.Web.Models
{
  public class AnalysisRequestAndResults {
    public AnalysisRequest Request { get; set; }
    public PlotlyChart ProjectTotalLibYearOverTime { get; set; }
    public PlotlyChart ProjectAverageLibYearOverTime { get; set; }
    public PlotlyChart ProjectMaxLibYearOverTime { get; set; }
    public PlotlyChart DependenciesLibYearOverTimeStacked { get; set; }

    private Dictionary<AnalysisRequestStatus, string> _statusMessages =
      new Dictionary<AnalysisRequestStatus, string>
      {
        {
          AnalysisRequestStatus.New,
          "We've received your request and we'll get started on it shortly."
        },
        {
          AnalysisRequestStatus.InProgress,
          "We're working on your request. Results will be available soon."
        },
        {
          AnalysisRequestStatus.Error,
          "We ran into a snag trying to process your request. We've " +
            "filed this away so that we can fix the issue. We'll contact " +
            "you when we've gotten the problem sorted out."
        },
        {
          AnalysisRequestStatus.Invalid,
          "We couldn't find any dependency manifests that we support. " +
            "We've filed this away so that we can investigate and " +
            "potentially add support for this repository."
        },
        {
          AnalysisRequestStatus.Retrying,
          "We're working on your request again. Results will be available soon."
        },
        {
          AnalysisRequestStatus.Success,
          "Your results are ready. Check them out!"
        }
      };

    public string StatusMessage => _statusMessages[Request.State];
  }
}
