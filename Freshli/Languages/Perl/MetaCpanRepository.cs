using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using Elasticsearch.Net;
using Freshli.Exceptions;

namespace Freshli.Languages.Perl {
  public class MetaCpanRepository : IPackageRepository {
    private IDictionary<string, IList<IVersionInfo>> _packages =
      new Dictionary<string, IList<IVersionInfo>>();

    private IList<IVersionInfo> GetReleaseHistory(string name) {
      if (_packages.ContainsKey(name)) {
        return _packages[name];
      }

      var settings =
        new ConnectionConfiguration(new Uri("https://fastapi.metacpan.org/v1")).
          RequestTimeout(TimeSpan.FromMinutes(2));

      var lowlevelClient = new ElasticLowLevelClient(settings);

      var searchResponse = lowlevelClient.Search<StringResponse>(
        "release",
        PostData.Serializable(
          new {
            from = 0,
            size = 500,
            fields = new[] {"distribution", "date", "version"},
            filter = new {term = new {distribution = name.Replace("::", "-")}},
            sort = new[] {new {date = new {order = "desc"}}}
          }
        )
      );

      if (!searchResponse.Success) {
        return null;
      }

      using var responseJson = JsonDocument.Parse(searchResponse.Body);
      if (!responseJson.RootElement.TryGetProperty("hits", out var hitsJson)) {
        return null;
      }

      var totalItems = hitsJson.GetProperty("total").GetInt32();
      // grab the next page if {totalItems} is greater than the page size

      var versions = new List<IVersionInfo>();
      foreach (var hit in hitsJson.GetProperty("hits").EnumerateArray()) {
        var fields = hit.GetProperty("fields");
        var version = fields.GetProperty("version").GetString();
        var date = DateTime.Parse(
          fields.GetProperty("date").GetString(),
          CultureInfo.InvariantCulture,
          DateTimeStyles.AssumeUniversal
        ).ToUniversalTime();

        versions.Add(new SemVerVersionInfo(version, date));
      }

      _packages[name] = versions;
      return versions;
    }

    public IVersionInfo Latest(string name, DateTime asOf) {
      return GetReleaseHistory(name).OrderByDescending(v => v).
        First(v => asOf >= v.DatePublished);
    }

    public IVersionInfo VersionInfo(string name, string version) {
      return GetReleaseHistory(name).First(v => v.Version == version);
    }

    public IVersionInfo Latest(string name, DateTime asOf, string thatMatches) {
      var expression = VersionMatcher.Create(thatMatches);
      return GetReleaseHistory(name).OrderByDescending(v => v).
        Where(v => v.DatePublished <= asOf).
        First(v => expression.DoesMatch(v));
    }

    public List<IVersionInfo> VersionsBetween(
      string name, IVersionInfo earlierVersion, IVersionInfo laterVersion)
    {
      try {
        return GetReleaseHistory(name).
          OrderByDescending(v => v).
          Where(predicate: v => v.CompareTo(earlierVersion) == 1).
          Where(predicate: v => v.CompareTo(laterVersion) == -1).ToList();
      }
      catch (Exception e) {
        throw new VersionsBetweenNotFoundException(
          name, earlierVersion.Version, laterVersion.Version, e);
      }
    }
  }
}
