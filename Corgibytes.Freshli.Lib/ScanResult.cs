using System;
using System.Collections.Generic;
using System.Text;

namespace Corgibytes.Freshli.Lib
{
    public record ScanResult(string Filename, List<MetricsResult> MetricResults)
    {
        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append($"Filename: {Filename}\n");
            sb.Append($"Results:\n");
            foreach (var result in MetricResults)
            {
                sb.Append(result.ToString());
            }
            return sb.ToString();
        }
    }
}
