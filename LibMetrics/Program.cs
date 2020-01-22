using System;
using System.IO;
using LibMetrics.Languages.Php;
using LibMetrics.Languages.Ruby;

namespace LibMetrics
{
    class Program
    {
        static void Main(string[] args)
        {
          ManifestFinder.Register<RubyBundlerManifestFinder>();
          ManifestFinder.Register<PhpComposerManifestFinder>();

          FileHistoryFinder.Register<GitFileHistoryFinder>();

          var runner = new Runner();
          var directory = Path.GetFullPath(args[0]);

          Console.Error.WriteLine($"Collecting data for {directory}");

          var results = runner.Run(args[0]);

          var formatter = new OutputFormatter(Console.Out);
          formatter.Write(results);
        }
    }
}
