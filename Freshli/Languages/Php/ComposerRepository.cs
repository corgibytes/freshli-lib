using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using RestSharp;

namespace Freshli.Languages.Php
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
      if (content == null) return null;

      using var responseJson = JsonDocument.Parse(content);
      if (!responseJson.RootElement.TryGetProperty("packages", out var packagesJson))
      {
        return null;
      }

      if (!packagesJson.TryGetProperty(name, out var versionsJson))
      {
        return null;
      }
      var versionsJsonElements = versionsJson.EnumerateObject();


      var foundVersions = new List<(string Version, DateTime PublishedAt)>();
      foreach (var versionJson in versionsJsonElements)
      {
        if (IsUnstable(versionJson.Name))
        {
          continue;
        }

        var version = versionJson.Name;

        var publishedDate = ParsePublishedDate(versionJson.Value);
        foundVersions.Add((version, publishedDate.Date));
      }

      foundVersions.Sort((left, right) =>
        left.CompareTo(right) | left.PublishedAt.CompareTo(right.PublishedAt)
      );
      var filteredVersions = foundVersions.Where(item => item.PublishedAt <= date).ToArray();
      if (!filteredVersions.Any()) return null;
      var selectedItem = filteredVersions.Last();
      return new VersionInfo(selectedItem.Version, selectedItem.PublishedAt);
    }

    private static DateTime ParsePublishedDate(JsonElement versionJson)
    {
      DateTime result = DateTime.MinValue;

      if (versionJson.TryGetProperty("time", out var standardTime))
      {
        result = DateTime.Parse(standardTime.GetString()).Date;
      }
      else if (versionJson.TryGetProperty("extra", out var extraData))
      {
        var datestamp = extraData.GetProperty("drupal").GetProperty("datestamp").GetString();
        result = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(datestamp)).Date.Date;
      }

      return result.Date;
    }

    public VersionInfo VersionInfo(string name, string version)
    {
      var content = FetchPackageInfo(name);
      if (content == null) return null;

      using var responseJson = JsonDocument.Parse(content);
      if (!responseJson.RootElement.TryGetProperty("packages", out var packagesJson))
      {
        return null;
      }

      if (!packagesJson.TryGetProperty(name, out var versionsJson))
      {
        return null;
      }
      var versionsJsonElements = versionsJson.EnumerateObject();

      if (versionsJson.TryGetProperty(version, out var versionJson))
      {
        var publishedDate = ParsePublishedDate(versionJson);

        return new VersionInfo(version, publishedDate.Date);
      }

      return null;
    }

    public VersionInfo Latest(string name, string thatMatches, DateTime asOf)
    {
      throw new NotImplementedException();
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

      if (!_packageInfoCache.ContainsKey(name))
      {
        return null;
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
