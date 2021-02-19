using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CliWrap;
using CliWrap.Buffered;

namespace Freshli {
  public class GitCliRepository {
    public string RepositoryRoot { get; private set; }
    public static bool IsValid(string path) {
      return Directory.Exists(Path.Combine(path, ".git"));
    }

    public static bool IsCloneable(string repositoryUrl) {
      // git ls-remote ${repositoryUrl}
      var command = Cli.Wrap("git").
        WithArguments(
          args => args.
            Add("ls-remote").
            Add(repositoryUrl)
        ).WithValidation(CommandResultValidation.None);
      var result = command.ExecuteAsync();
      return result.Task.Result.ExitCode == 0;
    }

    public static void Clone(string repositoryUrl, string path) {
      // git clone ${repositoryUrl} ${path}
      var command = Cli.Wrap("git").
        WithArguments(
          args => args.
            Add("clone").
            Add(repositoryUrl).
            Add(path)
        );
      command.ExecuteAsync().Task.Wait();
    }

    public GitCliRepository(string repositoryRoot) {
      RepositoryRoot = repositoryRoot;
    }

    public Dictionary<DateTime, string> LogEntriesFor(string path) {
      // Run in ${repositoryRoot}
      // git log \
      //   --full-history \
      //   --topo-order \
      //   --date=iso-strict \
      //   --pretty=format:"%cd %H" \
      //   -- ${path}

      var command = Cli.Wrap("git").
        WithWorkingDirectory(RepositoryRoot).
        WithArguments(
          args => args.
            Add("log").
            Add("--full-history").
            Add("--topo-order").
            Add("--date=iso-strict").
            Add("--pretty=format:%cd %H").
            Add("--").
            Add(path)
        );
      var output = command.ExecuteBufferedAsync().Task.Result.StandardOutput;
      var lines = output.Split(
        new[] {"\r\n", "\r", "\n"},
        StringSplitOptions.None
      );

      var result = new Dictionary<DateTime, string>();
      foreach (var line in lines) {
        var splits = line.Trim().Split(" ");
        var date = DateTimeOffset.Parse(
          splits[0],
          null,
          DateTimeStyles.RoundtripKind
        ).DateTime;
        result.Add(date, splits[1]);
      }

      return result;
    }

    public string FileContentsFromSha(string filePath, string sha) {
      // Run in ${repositoryRoot}
      // git show ${sha}:${filePath}

      var command = Cli.Wrap("git").
        WithWorkingDirectory(RepositoryRoot).
        WithArguments(
          args => args.
            Add("show").
            Add($"{sha}:{filePath}")
        );
      return command.ExecuteBufferedAsync().Task.Result.StandardOutput;
    }
  }
}
