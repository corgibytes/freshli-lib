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

    private Dictionary<AnalysisRequest.Status, string> _statusMessages =
      new Dictionary<AnalysisRequest.Status, string>
      {
        {
          AnalysisRequest.Status.New,
          "We've received your request and we'll get started on it shortly."
        },
        {
          AnalysisRequest.Status.InProgress,
          "We're working on your request. Results will be available soon."
        },
        {
          AnalysisRequest.Status.Error,
          "We ran into a snag trying to process your request. We've " +
            "filed this away so that we can fix the issue. We'll contact " +
            "you when we've gotten the problem sorted out."
        },
        {
          AnalysisRequest.Status.Invalid,
          "We couldn't find any dependency manifests that we support. " +
            "We've filed this away so that we can investigate and " +
            "potentially add support for this repository."
        },
        {
          AnalysisRequest.Status.Retrying,
          "We're working on your request again. Results will be available soon."
        },
        {
          AnalysisRequest.Status.Success,
          "Your results are ready. Check them out!"
        }
      };

    public string StatusMessage => _statusMessages[Request.State];
  }
}
