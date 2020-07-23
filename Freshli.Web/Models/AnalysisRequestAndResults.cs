using XPlot.Plotly;

namespace Freshli.Web.Models
{
  public class AnalysisRequestAndResults
  {
    public AnalysisRequest Request { get; set; }
    public PlotlyChart TotalLineChart { get; set; }
  }
}
