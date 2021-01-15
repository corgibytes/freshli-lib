using System;
using System.IO;
using NLog;

namespace Freshli.cli {
  class Program {
    private static readonly Logger logger = LogManager.GetCurrentClassLogger();

    static void Main(string[] args) {
      logger.Info($"Main({string.Join(separator: ",", args)})");

      try {
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

    private static string SafelyGetFullPath(string path) {
      var result = path;
      if (File.Exists(path)) {
        result = Path.GetFullPath(path);
      }

      return result;
    }
  }
}
