using XPlot.Plotly;

namespace Freshli.Web.Models
{
  public class AnalysisRequestAndResults {
    public AnalysisRequest Request { get; set; }
    public PlotlyChart ProjectTotalLibYearOverTime { get; set; }
    public PlotlyChart ProjectAverageLibYearOverTime { get; set; }
    public PlotlyChart ProjectMaxLibYearOverTime { get; set; }
    public PlotlyChart DependenciesLibYearOverTimeStacked { get; set; }

  }
}
