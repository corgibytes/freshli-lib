using System;

namespace LibMetrics
{
  public class MetricsResult
  {
    public DateTime Date;
    public LibYearResult LibYear;

    public MetricsResult(DateTime date, LibYearResult libYear)
    {
      Date = date;
      LibYear = libYear;
    }

    public MetricsResult(DateTime date, double libYearValue)
    {
      Date = date;
      LibYear = new LibYearResult(libYearValue);
    }
  }
}
