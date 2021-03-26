using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using NLog;
using RestSharp;

namespace Corgibytes.Freshli.Lib.Languages.Php {
  public class ComposerRepository : IPackageRepository {
    private readonly string _baseUrl;

    private Dictionary<string, string> _packageInfoCache =
      new Dictionary<string, string>();

    private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

    public ComposerRepository(string baseUrl) {
      _baseUrl = baseUrl;
    }

    //TODO: Update logic to utilize includePreReleases
    public IVersionInfo Latest(
      string name,
      DateTimeOffset asOf,
      bool includePreReleases)
    {
      var content = FetchPackageInfo(name);
      if (content == null) return null;

      using var responseJson = JsonDocument.Parse(content);
      if (!responseJson.RootElement.TryGetProperty(
        "packages",
        out var packagesJson
      )) {
        return null;
      }

      if (!packagesJson.TryGetProperty(name, out var versionsJson)) {
        return null;
      }

      var versionsJsonElements = versionsJson.EnumerateObject();

      var foundVersions = new List<SemVerVersionInfo>();
      foreach (var versionJson in versionsJsonElements) {
        if (IsUnstable(versionJson.Name)) {
          continue;
        }

        var version = versionJson.Name;
        var publishedDate = ParsePublishedDate(versionJson.Value);
        var versionInfo = new SemVerVersionInfo(version, publishedDate);
        if (versionInfo.PreRelease != null) {
          continue;
        }

        foundVersions.Add(versionInfo);
      }

      foundVersions.Sort(
        (left, right) =>
          left.CompareTo(right)
      );
      var filteredVersions = foundVersions.
        Where(item => item.DatePublished <= asOf).ToArray();
      if (!filteredVersions.Any()) return null;
      return filteredVersions.Last();
    }

    private static DateTimeOffset ParsePublishedDate(JsonElement versionJson) {
      var result = DateTimeOffset.MinValue;

      if (versionJson.TryGetProperty("time", out var standardTime)) {
        result = DateTimeOffset.Parse(standardTime.GetString());
      } else if (versionJson.TryGetProperty("extra", out var extraData)) {
        var datestamp = extraData.GetProperty("drupal").
          GetProperty("datestamp").GetString();
        result = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(datestamp));
      }

      return result;
    }

    public IVersionInfo VersionInfo(string name, string version) {
      var content = FetchPackageInfo(name);
      if (content == null) return null;

      using var responseJson = JsonDocument.Parse(content);
      if (!responseJson.RootElement.TryGetProperty(
        "packages",
        out var packagesJson
      )) {
        return null;
      }

      if (!packagesJson.TryGetProperty(name, out var versionsJson)) {
        return null;
      }

      if (versionsJson.TryGetProperty(version, out var versionJson)) {
        var publishedDate = ParsePublishedDate(versionJson);

        return new SemVerVersionInfo(version, publishedDate);
      }

      return null;
    }

    public IVersionInfo Latest(
      string name,
      DateTimeOffset asOf,
      string thatMatches
    ) {
      throw new NotImplementedException();
    }

    public List<IVersionInfo> VersionsBetween(
      string name,
      DateTimeOffset asOf,
      IVersionInfo earlierVersion,
      IVersionInfo laterVersion,
      bool includePreReleases)
    {
      //TODO: Implement method
      throw new NotImplementedException();
    }

    private string FetchPackageInfo(string name) {
      if (!_packageInfoCache.ContainsKey(name)) {
        var packageListing = Request("/packages.json");

        using var packageJson = JsonDocument.Parse(packageListing);
        var urlTemplate = packageJson.RootElement.GetProperty("providers-url").
          GetString();

        var providerIncludes = packageJson.RootElement.
          GetProperty("provider-includes").EnumerateObject();
        foreach (var providerInclude in providerIncludes) {
          var providerName = providerInclude.Name;
          var providerHash = providerInclude.Value.GetProperty("sha256").
            GetString();

          var providerListing =
            Request($"/{providerName.Replace("%hash%", providerHash)}");
          using var providerListingJson = JsonDocument.Parse(providerListing);
          JsonElement packageProvider;
          if (providerListingJson.RootElement.GetProperty("providers").
            TryGetProperty(name, out packageProvider)) {
            var packageHash = packageProvider.GetProperty("sha256").GetString();
            var packageUrl = urlTemplate.
              Replace("%package%", name).
              Replace("%hash%", packageHash);
            var packageDetails = Request($"/{packageUrl}");
            _logger.Trace(
              $"{name} package info loaded from {_baseUrl}{packageUrl}"
            );

            _packageInfoCache[name] = packageDetails;
            break;
          }
        }
      }

      if (!_packageInfoCache.ContainsKey(name)) {
        return null;
      }

      return _packageInfoCache[name];
    }

    private Dictionary<string, string> _requestCache =
      new Dictionary<string, string>();

    private string Request(string url) {
      if (!_requestCache.ContainsKey(url)) {
        var client = new RestClient(_baseUrl);
        var request = new RestRequest(url);
        var response = client.Execute(request);

        _requestCache[url] = response.Content;
      }

      return _requestCache[url];
    }

    private bool IsUnstable(string version) {
      return IsDev(version) ||
        IsAlpha(version) ||
        IsBeta(version) ||
        IsReleaseCandidate(version);
    }

    private bool IsDev(string version) {
      return version.StartsWith("dev-") || version.EndsWith("-dev");
    }

    private bool IsAlpha(string version) {
      var expression = new Regex(@"-alpha\d*$");
      return expression.IsMatch(version);
    }

    private bool IsBeta(string version) {
      var expression = new Regex(@"-beta\d*$");
      return expression.IsMatch(version);
    }

    private bool IsReleaseCandidate(string version) {
      var expression = new Regex(@"-RC\d*$");
      return expression.IsMatch(version);
    }
  }
}
