using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using Elasticsearch.Net;

namespace Freshli.Languages.Perl {
  public class MetaCpanRepository: IPackageRepository {
    private IDictionary<string, IList<VersionInfo>> _packages =
      new Dictionary<string, IList<VersionInfo>>();
    
    private IList<VersionInfo> GetReleaseHistory(string name) {
      if (_packages.ContainsKey(name)) {
        return _packages[name];
      }
      
      var settings = new ConnectionConfiguration(new Uri("https://fastapi.metacpan.org/v1"))
        .RequestTimeout(TimeSpan.FromMinutes(2));
      
      var lowlevelClient = new ElasticLowLevelClient(settings);
      
      var searchResponse = lowlevelClient.Search<StringResponse>(
        "release", 
        PostData.Serializable(new {
          from = 0,
          size = 500,
          fields = new[] {
            "distribution", "date", "version"
          },
          filter = new {
            term = new {
              distribution = name.Replace("::", "-")
            }
          },
          sort = new[] {
            new {
              date = new { order = "desc" }
            }
          }
        }));

      if (!searchResponse.Success) {
        return null;
      }

      using var responseJson = JsonDocument.Parse(searchResponse.Body);
      if (!responseJson.RootElement.TryGetProperty("hits", out var hitsJson)) {
        return null;
      }

      var totalItems = hitsJson.GetProperty("total").GetInt32();
      // grab the next page if {totalItems} is greater than the page size
      
      var versions = new List<VersionInfo>();
      foreach (var hit in hitsJson.GetProperty("hits").EnumerateArray()) {
        var fields = hit.GetProperty("fields");
        var version = fields.GetProperty("version").GetString();
        var date = DateTime.Parse(
          fields.GetProperty("date").GetString(), 
          CultureInfo.InvariantCulture, 
          DateTimeStyles.AssumeUniversal).ToUniversalTime();
        
        versions.Add(new VersionInfo(version, date));
      }

      _packages[name] = versions;
      return versions;
    }
    
    public VersionInfo LatestAsOf(DateTime date, string name) {
      return GetReleaseHistory(name).OrderByDescending(v => v).
        First(v => date >= v.DatePublished);
    }

    public VersionInfo VersionInfo(string name, string version) {
      return GetReleaseHistory(name).First(v => v.Version == version);
    }

    public VersionInfo Latest(string name, string thatMatches, DateTime asOf) {
      var expression = VersionMatcher.Create(thatMatches);
      return GetReleaseHistory(name).OrderByDescending(v => v).
        Where(v => v.DatePublished <= asOf).
        First(v => expression.DoesMatch(v));
    }
  }
}
