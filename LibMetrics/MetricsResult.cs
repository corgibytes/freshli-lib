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

    public override string ToString()
    {
      return $"{{ Date: {Date}, LibYear: {LibYear} }}";
    }
  }
}
