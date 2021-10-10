﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Corgibytes.Freshli.Lib.Exceptions;
using Elasticsearch.Net;

namespace Corgibytes.Freshli.Lib.Languages.Perl
{
    public class MetaCpanRepository : IPackageRepository
    {
        private IDictionary<string, IList<IVersionInfo>> _packages =
          new Dictionary<string, IList<IVersionInfo>>();

        private async Task<IList<IVersionInfo>> GetReleaseHistory(string name)
        {
            if (_packages.ContainsKey(name))
            {
                return _packages[name];
            }

            var settings =
                new ConnectionConfiguration(new Uri("https://fastapi.metacpan.org/v1")).
                    RequestTimeout(TimeSpan.FromMinutes(2));

            var lowlevelClient = new ElasticLowLevelClient(settings);

            var searchResponse = await lowlevelClient.SearchAsync<StringResponse>(
                "release",
                PostData.Serializable(
                    new
                    {
                        from = 0,
                        size = 500,
                        fields = new[] { "distribution", "date", "version" },
                        filter = new { term = new { distribution = name.Replace("::", "-") } },
                        sort = new[] { new { date = new { order = "desc" } } }
                    }
                )
            );

            if (!searchResponse.Success)
            {
                return null;
            }

            using var responseJson = JsonDocument.Parse(searchResponse.Body);
            if (!responseJson.RootElement.TryGetProperty("hits", out var hitsJson))
            {
                return null;
            }

            var versions = new List<IVersionInfo>();
            foreach (var hit in hitsJson.GetProperty("hits").EnumerateArray())
            {
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

        //TODO: Update logic to utilize includePreReleases
        public IVersionInfo Latest(
            string name,
            DateTime asOf,
            bool includePreReleases)
        {
            return GetReleaseHistory(name).Result.OrderByDescending(v => v).
                First(v => asOf >= v.DatePublished);
        }

        public async Task<IVersionInfo> VersionInfo(string name, string version)
        {
            return (await GetReleaseHistory(name)).First(v => v.Version == version);
        }

        public IVersionInfo Latest(string name, DateTime asOf, string thatMatches)
        {
            var expression = VersionMatcher.Create(thatMatches);
            return GetReleaseHistory(name).Result.OrderByDescending(v => v).
                Where(v => v.DatePublished <= asOf).
                First(v => expression.DoesMatch(v));
        }

        //TODO: Update logic to utilize includePreReleases
        public List<IVersionInfo> VersionsBetween(string name, DateTime asOf,
            IVersionInfo earlierVersion, IVersionInfo laterVersion,
            bool includePreReleases)
        {
            try
            {
                return GetReleaseHistory(name).
                    Result.
                    OrderByDescending(v => v).
                    Where(v => v.DatePublished <= asOf).
                    Where(predicate: v => v.CompareTo(earlierVersion) == 1).
                    Where(predicate: v => v.CompareTo(laterVersion) == -1).ToList();
            }
            catch (Exception e)
            {
                throw new VersionsBetweenNotFoundException(
                    name, earlierVersion.Version, laterVersion.Version, e);
            }
        }
    }
}
