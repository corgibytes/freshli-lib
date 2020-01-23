using System;
using System.Collections.Generic;
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
        var client = new RestClient("https://repo.packagist.org/p");
        var request = new RestRequest($"/{name}.json");
        var response = client.Execute(request);

        _packageInfoCache[name] = response.Content;
      }
      return _packageInfoCache[name];
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
