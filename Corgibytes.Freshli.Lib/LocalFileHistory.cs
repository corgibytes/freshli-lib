using System;
using System.Collections.Generic;
using System.IO;

namespace Corgibytes.Freshli.Lib
{
    public class LocalFileHistory : IFileHistory
    {
        private readonly string _rootDirectory;
        private readonly string _targetPath;

        // This value is cached in this way to ensure that all files are
        // served up with the same date. This could be made more sophisticated
        // by making the value dependent on the `_rootDirectory` and/or
        // `targetPath` values. But this should be good enough for now.
        private static readonly DateTimeOffset _now = DateTimeOffset.UtcNow;

        public LocalFileHistory(string rootDirectory, string targetPath)
        {
            _rootDirectory = rootDirectory;
            _targetPath = targetPath;
        }

        public IEnumerable<DateTimeOffset> Dates => new List<DateTimeOffset> { _now };

        public string ContentsAsOf(DateTimeOffset date)
        {
            using var contentStream = ContentStreamAsOf(date);
            using var reader = new StreamReader(contentStream);
            return reader.ReadToEnd();
        }

        public Stream ContentStreamAsOf(DateTimeOffset date)
        {
            return File.OpenRead(Path.Combine(_rootDirectory, _targetPath));
        }

        public string ShaAsOf(DateTimeOffset date)
        {
            return "N/A";
        }
    }
}
