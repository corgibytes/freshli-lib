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

    private Dictionary<AnalysisRequestState, string> _statusMessages =
      new Dictionary<AnalysisRequestState, string>
      {
        {
          AnalysisRequestState.New,
          "We've received your request and we'll get started on it shortly."
        },
        {
          AnalysisRequestState.InProgress,
          "We're working on your request. Results will be available soon."
        },
        {
          AnalysisRequestState.Error,
          "We ran into a snag trying to process your request. We've " +
            "filed this away so that we can fix the issue. We'll contact " +
            "you when we've gotten the problem sorted out."
        },
        {
          AnalysisRequestState.Invalid,
          "We couldn't find any dependency manifests that we support. " +
            "We've filed this away so that we can investigate and " +
            "potentially add support for this repository."
        },
        {
          AnalysisRequestState.Retrying,
          "We're working on your request again. Results will be available soon."
        },
        {
          AnalysisRequestState.Success,
          "Your results are ready. Check them out!"
        }
      };

    public string StatusMessage => _statusMessages[Request.State];
  }
}
