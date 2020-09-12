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

    public IList<DateTime> Dates => new List<DateTime> {DateTime.Today};

    public string ContentsAsOf(DateTime date) {
      return File.ReadAllText(Path.Combine(_rootDirectory, _targetPath));
    }

    public string ShaAsOf(DateTime date) {
      return "N/A";
    }
  }
}
