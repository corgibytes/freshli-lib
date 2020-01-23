using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using RestSharp;

namespace LibMetrics.Languages.Php
{
  public class ComposerRepository: IPackageRepository
  {
    private readonly string _baseUrl;
    private Dictionary<string, string> _packageInfoCache =
      new Dictionary<string, string>();

    public ComposerRepository(string baseUrl)
    {
      _baseUrl = baseUrl;
    }

    public VersionInfo LatestAsOf(DateTime date, string name)
    {
      var content = FetchPackageInfo(name);
      using var responseJson = JsonDocument.Parse(content);
      var versionsJson = responseJson.RootElement.GetProperty("packages").GetProperty(name).EnumerateObject();

      var foundVersions = new List<(string Version, DateTime PublishedAt)>();
      foreach (var versionJson in versionsJson)
      {
        if (IsUnstable(versionJson.Name))
        {
          continue;
        }

        var version = versionJson.Name;
        var publishedDate = DateTime.Parse(versionJson.Value.GetProperty("time").GetString());
        foundVersions.Add((version, publishedDate.Date));
      }

      foundVersions.Sort((left, right) => left.PublishedAt.CompareTo(right.PublishedAt));
      var selectedItem = foundVersions.Last(item => item.PublishedAt <= date);
      return new VersionInfo(selectedItem.Version, selectedItem.PublishedAt);
    }

    public VersionInfo VersionInfo(string name, string version)
    {
      var content = FetchPackageInfo(name);
      using var responseJson = JsonDocument.Parse(content);
      var versionsJson = responseJson.RootElement.GetProperty("packages").GetProperty(name);

      var versionJson = versionsJson.GetProperty(version);

      var publishedDate = DateTime.Parse(versionJson.GetProperty("time").GetString());

      return new VersionInfo(version, publishedDate.Date);
    }

    private string FetchPackageInfo(string name)
    {
      if (!_packageInfoCache.ContainsKey(name))
      {
        var packageListing = Request("/packages.json");

        using var packageJson = JsonDocument.Parse(packageListing);
        var urlTemplate = packageJson.RootElement.GetProperty("providers-url").
          GetString();

        var providerIncludes = packageJson.RootElement.
          GetProperty("provider-includes").EnumerateObject();
        foreach (var providerInclude in providerIncludes)
        {
          var providerName = providerInclude.Name;
          var providerHash = providerInclude.Value.GetProperty("sha256").
            GetString();

          var providerListing =
            Request($"/{providerName.Replace("%hash%", providerHash)}");
          using var providerListingJson = JsonDocument.Parse(providerListing);
          JsonElement packageProvider;
          if (providerListingJson.RootElement.GetProperty("providers").
            TryGetProperty(name, out packageProvider))
          {
            var packageHash = packageProvider.GetProperty("sha256").GetString();
            var packageUrl = urlTemplate.
              Replace("%package%", name).
              Replace("%hash%", packageHash);
            var packageDetails = Request($"/{packageUrl}");

            _packageInfoCache[name] = packageDetails;
            break;
          }
        }
      }
      return _packageInfoCache[name];
    }

    private Dictionary<string, string> _requestCache =
      new Dictionary<string, string>();
    private string Request(string url)
    {
      if (!_requestCache.ContainsKey(url))
      {
        var client = new RestClient(_baseUrl);
        var request = new RestRequest(url);
        var response = client.Execute(request);

        _requestCache[url] = response.Content;
      }

      return _requestCache[url];
    }

    private bool IsUnstable(string version)
    {
      return IsDev(version) ||
             IsAlpha(version) ||
             IsBeta(version) ||
             IsReleaseCandidate(version);
    }

    private bool IsDev(string version)
    {
      return version.StartsWith("dev-") || version.EndsWith("-dev");
    }

    private bool IsAlpha(string version)
    {
      var expression = new Regex(@"-alpha\d*$");
      return expression.IsMatch(version);
    }

    private bool IsBeta(string version)
    {
      var expression = new Regex(@"-beta\d*$");
      return expression.IsMatch(version);
    }

    private bool IsReleaseCandidate(string version)
    {
      var expression = new Regex(@"-RC\d*$");
      return expression.IsMatch(version);
    }
  }
}
