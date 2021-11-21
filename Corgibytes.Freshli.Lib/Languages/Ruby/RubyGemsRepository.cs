using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Xml.Linq;
using Corgibytes.Freshli.Lib.Exceptions;
using HtmlAgilityPack;
using Polly;

namespace Corgibytes.Freshli.Lib.Languages.Ruby
{
    public class RubyGemsRepository : IPackageRepository
    {
        // TODO: migrate this to towards something from Microsoft.Extensions.Caching
        private static IDictionary<string, IList<IVersionInfo>> _packages = new Dictionary<string, IList<IVersionInfo>>();

        private IEnumerable<IVersionInfo> GetReleaseHistory(string name, bool includePreReleaseVersions)
        {
            try
            {
                var key = BuildCacheKey(name, includePreReleaseVersions);
                if (_packages.ContainsKey(key))
                {
                    return _packages[key];
                }

                var versions = GetReleaseHistoryForGem(name);
                versions.RemoveAll(version => version.IsPlatformSpecific);

                if (!includePreReleaseVersions)
                {
                    versions.RemoveAll(version => version.IsPreRelease);
                }

                _packages.Add(key, versions.Cast<IVersionInfo>().ToList());
                return versions;

            }
            catch (Exception e)
            {
                throw new DependencyNotFoundException(name, e);
            }
        }

        private string BuildCacheKey(string name, bool includePreReleaseVersions)
        {
            var keySuffix = includePreReleaseVersions ? "-with-pre-releases" : "";
            var key = $"{name}{keySuffix}";
            return key;
        }

        private List<RubyGemsVersionInfo> GetReleaseHistoryForGem(string name)
        {
            var url = $"https://rubygems.org/gems/{name}/versions.atom";
            var document = Policy.Handle<System.Net.Http.HttpRequestException>().
              WaitAndRetry(
                5,
                retryAttempt =>
                  TimeSpan.FromMilliseconds(100 * Math.Pow(2, retryAttempt))
              ).ExecuteAndCapture(() => XDocument.Load(url)).Result;
            XNamespace atom = "http://www.w3.org/2005/Atom";

            var versions = document.Descendants(atom + "entry").
                Select(entry => new RubyGemsVersionInfo(
                    entry.Descendants(atom + "id").First().Value.Split("/").Last(),
                    DateTimeOffset.Parse(
                    entry.Descendants(atom + "updated").First().Value
                ))
            ).ToList();
            return versions;
        }

        public IVersionInfo Latest(string name, DateTimeOffset asOf, bool includePreReleases)
        {
            try
            {
                return GetReleaseHistory(name, includePreReleases).
                    OrderByDescending(v => v).
                    First(v => asOf >= v.DatePublished);
            }
            catch (Exception e)
            {
                throw new LatestVersionNotFoundException(name, asOf, e);
            }
        }

        public IVersionInfo VersionInfo(string name, string version)
        {
            try
            {
                return GetReleaseHistory(name, includePreReleaseVersions: true).
                    First(v => v.Version == version);
            }
            catch (Exception e)
            {
                throw new VersionNotFoundException(name, version, e);
            }
        }

        public IVersionInfo Latest(string name, DateTimeOffset asOf, string thatMatches)
        {
            // TODO: implement this method
            throw new NotImplementedException();
        }

        public List<IVersionInfo> VersionsBetween(string name, DateTimeOffset asOf, IVersionInfo earlierVersion, IVersionInfo laterVersion, bool includePreReleases)
        {
            try
            {
                return GetReleaseHistory(name, includePreReleases).
                    OrderByDescending(v => v).
                    Where(v => asOf >= v.DatePublished).
                    Where(predicate: v => v.CompareTo(earlierVersion) == 1).
                    Where(predicate: v => v.CompareTo(laterVersion) == -1).ToList();
            }
            catch (Exception e)
            {
                throw new VersionsBetweenNotFoundException(name, earlierVersion.Version, laterVersion.Version, e);
            }
        }

        private bool IsReleasePlatformSpecific(HtmlNode node)
        {
            var platformSpecific = false;
            foreach (var span in node.Descendants("span"))
            {
                foreach (var className in span.GetClasses())
                {
                    if (className == "platform")
                    {
                        platformSpecific = true;
                    }
                }
            }
            return platformSpecific;
        }

        // TODO: Is it safe to delete this?
        private void ProcessReleaseNode(bool includePreReleaseVersions, List<IVersionInfo> versions, HtmlNode releaseNode)
        {
            var version = releaseNode.Descendants("a").
                        First(a => a.HasClass("t-list__item")).InnerText;

            var rawDate = releaseNode.Descendants("small").First().InnerText.
                Replace("- ", "");
            var versionDate = DateTimeOffset.ParseExact(
                rawDate,
                "MMMM dd, yyyy",
                CultureInfo.InvariantCulture,
                DateTimeStyles.AssumeUniversal
            );

            var versionInfo = new RubyGemsVersionInfo(version, versionDate);
            if ((!versionInfo.IsPreRelease || includePreReleaseVersions) &&
                !IsReleasePlatformSpecific(releaseNode))
            {
                versions.Add(versionInfo);
            }
        }
    }
}
