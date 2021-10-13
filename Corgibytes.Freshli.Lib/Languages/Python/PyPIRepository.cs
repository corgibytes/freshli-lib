using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Corgibytes.Freshli.Lib.Exceptions;
using HtmlAgilityPack;
using NLog;
using Polly;

namespace Corgibytes.Freshli.Lib.Languages.Python
{
    public class PyPIRepository : IPackageRepository
    {

        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private IDictionary<string, IList<IVersionInfo>> _packages =
            new Dictionary<string, IList<IVersionInfo>>();

        private async Task<IList<IVersionInfo>> GetReleaseHistory(string name)
        {
            try
            {
                if (_packages.ContainsKey(name))
                {
                    return _packages[name];
                }

                var url = $"https://pypi.org/p/{name}";
                var web = new HtmlWeb();

                // TODO: Setup this policy in a centralized location
                var policy = Policy.BulkheadAsync(5);
                var doc = await policy.ExecuteAsync(
                    async cancelizationToken => await web.LoadFromWebAsync(url, cancelizationToken),
                    CancellationToken.None
                );

                var versions = new List<IVersionInfo>();

                var releaseNodes = doc.DocumentNode.Descendants("div").
                    Where(div => div.HasClass("release"));

                foreach (var releaseNode in releaseNodes)
                {
                    var versionNode = releaseNode.Descendants("p").
                        First(p => p.HasClass("release__version"));
                    var rawVersion = Regex.Replace(versionNode.InnerText, @"\s+", " ").
                        Trim();

                    var versionSplits = rawVersion.Split(' ');

                    if (versionSplits.Length > 1)
                    {
                        continue;
                    }

                    var version = versionSplits[0];

                    var dateNode = releaseNode.Descendants("time").First();
                    var versionDate = DateTime.Parse(
                        dateNode.Attributes["datetime"].Value).ToUniversalTime();

                    try
                    {
                        versions.Add(new PythonVersionInfo(version, versionDate));
                    }
                    catch (VersionParseException e)
                    {
                        _logger.Warn(
                            $"Error adding version to {name} release history: {e.Message}");
                    }
                }

                _packages[name] = versions;
                return versions;
            }
            catch (Exception e)
            {
                throw new DependencyNotFoundException(name, e);
            }
        }

        //TODO: Update logic to utilize includePreReleases
        public async Task<IVersionInfo> Latest(
            string name,
            DateTime asOf,
            bool includePreReleases)
        {
            try
            {
                return (await GetReleaseHistory(name)).OrderByDescending(v => v).
                    First(v => asOf >= v.DatePublished);
            }
            catch (VersionParseException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new LatestVersionNotFoundException(name, asOf, e);
            }
        }

        public async Task<IVersionInfo> VersionInfo(string name, string version)
        {
            return (await GetReleaseHistory(name)).First(v => v.Version == version);
        }

        public async Task<IVersionInfo> Latest(string name, DateTime asOf, string thatMatches)
        {
            try
            {
                var expression = VersionMatcher.Create(thatMatches);
                return (await GetReleaseHistory(name)).OrderByDescending(v => v).
                  Where(v => v.DatePublished <= asOf).
                  First(v => expression.DoesMatch(v));
            }
            catch (VersionParseException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new LatestVersionThatMatchesNotFoundException(
                  name, asOf, thatMatches, e);
            }
        }

        //TODO: Update logic to utilize includePreReleases
        public async Task<List<IVersionInfo>> VersionsBetween(
            string name,
            DateTime asOf,
            IVersionInfo earlierVersion,
            IVersionInfo laterVersion,
            bool includePreReleases)
        {
            try
            {
                return (await GetReleaseHistory(name)).
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
