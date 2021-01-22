using System;
using System.IO;
using System.Linq;
using NLog;

namespace Freshli {
  class Program {
    private static readonly Logger logger = LogManager.GetCurrentClassLogger();

    static void Main(string[] args) {
      ValidateArguments(args);

      logger.Info($"Main({string.Join(separator: ",", args)})");

      try {
        ManifestFinder.RegisterAll();

        FileHistoryFinder.Register<GitFileHistoryFinder>();

        var runner = new Runner();
        var directory = SafelyGetFullPath(args[0]);

        logger.Info($"Collecting data for {directory}");

        var results = runner.Run(args[0]);

        var formatter = new OutputFormatter(Console.Out);
        formatter.Write(results);
      } catch (Exception e) {
        logger.Error(
          e,
          $"Exception executing Freshli for args = " +
          $"[{string.Join(separator: ",", args)}]: {e.Message}"
        );
        logger.Trace(e, e.StackTrace);
      }
    }

    private static void ValidateArguments(string[] args) {
      if (!args.Any())
      {
        logger.Error($"Repository URL is required to run Freshli.");
        Environment.Exit(1);
      }
    }

    private static string SafelyGetFullPath(string path) {
      var result = path;
      if (File.Exists(path)) {
        result = Path.GetFullPath(path);
      }

      return result;
    }
  }
}
