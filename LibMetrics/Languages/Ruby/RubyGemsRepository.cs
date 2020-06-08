using System;
using System.Collections.Generic;
using System.Linq;
using RestSharp;

namespace LibMetrics.Languages.Ruby
{
  public class RubyGemsRepository : IPackageRepository
  {
    public VersionInfo LatestAsOf(DateTime date, string name)
    {
      var response = ApiRequest(name);

      var candidates = response.
        Where(version => !version.Prerelease && version.CreatedAt <= date).
        OrderByDescending(version => version.CreatedAt);

      var firstCandidate = candidates.FirstOrDefault();
      if (firstCandidate != null)
      {
        return new VersionInfo(
          firstCandidate.Number,
          firstCandidate.CreatedAt.Date);
      }

      return null;
    }

    private Dictionary<String, List<RubyGemsVersion>> _responseCache = new Dictionary<string, List<RubyGemsVersion>>();
    private List<RubyGemsVersion> ApiRequest(string name)
    {
      if (!_responseCache.ContainsKey(name))
      {
        var client = new RestClient("https://rubygems.org/api/v1");
        var request = new RestRequest($"/versions/{name}.json");
        var response = client.Execute<List<RubyGemsVersion>>(request);
        _responseCache[name] = response.Data;
      }

      return _responseCache[name];
    }

    public VersionInfo VersionInfo(string name, string version)
    {
      var response = ApiRequest(name);

      var candidate = response.FirstOrDefault(package => package?.Number == version);

      if (candidate != null)
      {
        return new VersionInfo(candidate.Number, candidate.CreatedAt.Date);
      }

      return null;
    }
  }
}
