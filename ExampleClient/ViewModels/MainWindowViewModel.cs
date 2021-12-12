using System;
using System.Reactive;
using System.Threading.Tasks;
using Corgibytes.Freshli.Lib;
using Microsoft.Extensions.Logging;
using NLog;
using ReactiveUI;
using Splat;
using Splat.Microsoft.Extensions.Logging;

namespace ExampleClient.ViewModels
{
    /// <summary>
    /// Allow developers to run Freshli Lib commands.
    /// </summary>
    public class MainWindowViewModel : ViewModelBase
    {
        /// <summary>
        /// The path to the Git repo to run Freshli against.
        /// </summary>
        public string GitPath { get; set; }

        /// <summary>
        /// Show the results from Freshli.
        /// </summary>
        public string Results
        {
            get => _results;
            set => this.RaiseAndSetIfChanged(ref _results, value);
        }
        private string _results;

        /// <summary>
        /// Close the window.
        /// </summary>
        public ReactiveCommand<Unit, Unit> CloseCommand { get; }

        /// <summary>
        /// Run Freshli.
        /// </summary>
        public ReactiveCommand<Unit, Unit> OnRunFreshliClick { get; }

        /// <summary>
        /// Initialize the view model.
        /// </summary>
        public MainWindowViewModel()
        {
            _results = "";

            GitPath = "";
            Results = "";

            OnRunFreshliClick = ReactiveCommand.Create(RunFreshli);
            CloseCommand = ReactiveCommand.Create(execute: () => { });
        }

        /// <summary>
        /// Run Freshli and show the results to the user.
        /// </summary>
        private void RunFreshli()
        {
            Results = $"Starting Freshli run for '{GitPath}'{Environment.NewLine}";

            var loggerFactory = new LoggerFactory();

            Locator.CurrentMutable.UseMicrosoftExtensionsLoggingWithWrappingFullLogger(loggerFactory);

            var manifestFinderRegistry = new ManifestFinderRegistry(loggerFactory.CreateLogger<ManifestFinderRegistry>());

            var loader = new ManifestFinderRegistryLoader(loggerFactory.CreateLogger<ManifestFinderRegistryLoader>());
            loader.RegisterAll(manifestFinderRegistry);

            var fileHistoryFinderRegistry = new FileHistoryFinderRegistry();
            fileHistoryFinderRegistry.Register<GitFileHistoryFinder>();
            fileHistoryFinderRegistry.Register<LocalFileHistoryFinder>();

            var runner = new Runner(manifestFinderRegistry, fileHistoryFinderRegistry, loggerFactory);
            var metricResults = runner.Run(GitPath);

            foreach (var mr in metricResults)
            {
                Results += mr.ToString();
            }

            Results += $"Finished!{Environment.NewLine}";
        }
    }
}
