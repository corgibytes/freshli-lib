using System;
using System.Collections.Generic;
using System.IO;

namespace Freshli {
  public class LocalFileHistory : IFileHistory {
    private readonly string _rootDirectory;
    private readonly string _targetPath;

    public LocalFileHistory(string rootDirectory, string targetPath) {
      _rootDirectory = rootDirectory;
      _targetPath = targetPath;
    }

    public IList<DateTimeOffset> Dates => new List<DateTimeOffset> {
      DateTime.Today
    };

    public string ContentsAsOf(DateTimeOffset date) {
      return File.ReadAllText(Path.Combine(_rootDirectory, _targetPath));
    }

    public string ShaAsOf(DateTimeOffset date) {
      return "N/A";
    }
  }
}
