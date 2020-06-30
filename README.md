[![.NET Core](https://github.com/corgibytes/freshli/workflows/.NET%20Core/badge.svg)](https://github.com/corgibytes/freshli/actions?query=workflow%3A%22.NET+Core%22) [![EditorConfig Lint](https://github.com/corgibytes/freshli/workflows/EditorConfig%20Lint/badge.svg)](https://github.com/corgibytes/freshli/actions?query=workflow%3A%22EditorConfig+Lint%22)

# freshli
A CLI tool for collecting historical metrics about a project's dependencies

The `freshli` command line tool captures historical metrics about a project's dependencies. In it's current form, the only metric that it computes is [libyear](https://libyear.com/).

For each language that the tool supports, the libyear metric is computed for each month in the past where dependency information is available.

## Supported Tools

### Languages and Dependency Frameworks

* Ruby
  * bundler - reads information from `Gemfile.lock`
* Perl
  * carton - reads information from `cpanfile`
* PHP
  * composer - reads information from `composer.json` and `composer.lock`
* Python
  * pip - reads information from `requirements.txt`

### Source Code Repositories

* Git

## Getting started

### Running `freshli`

Right now to run `freshli` from the command line, you need to have the .NET Core SDK installed.

Once you do, you can run use `dotnet run --project Freshli/Freshli.csproj -- <url>` to run the project.

Here's an example:

```
➜  freshli git:(main) ✗ dotnet run --project Freshli/Freshli.csproj -- http://github.com/corgibytes/freshli-fixture-ruby-nokotest
VersionInfo.cs(201,32): warning CS8632: The annotation for nullable reference types should only be used in code within a '#nullable' annotations context. [/Users/mscottford/src/corgibytes/freshli/Freshli/Freshli.csproj]
2020/06/18 16:20:36.449| INFO|Freshli.Program:16|Main(System.String[])
2020/06/18 16:20:36.480| INFO|Freshli.Program:29|Collecting data for http://github.com/corgibytes/freshli-fixture-ruby-nokotest
2020/06/18 16:20:36.480| INFO|Freshli.Runner:13|Run(http://github.com/corgibytes/freshli-fixture-ruby-nokotest, 6/18/2020 12:00:00 AM)
2017/01/01	0.000
2017/02/01	0.022
2017/03/01	0.022
2017/04/01	0.227
2017/05/01	0.227
2017/06/01	0.364
2017/07/01	1.852
2017/08/01	1.852
2017/09/01	1.852
2017/10/01	2.416
2017/11/01	2.416
2017/12/01	2.416
2018/01/01	0.000
2018/02/01	0.362
2018/03/01	0.362
2018/04/01	0.362
2018/05/01	0.362
2018/06/01	0.362
2018/07/01	0.740
2018/08/01	0.789
2018/09/01	0.789
2018/10/01	0.789
2018/11/01	1.044
2018/12/01	1.044
2019/01/01	0.000
2019/02/01	0.071
2019/03/01	0.071
2019/04/01	0.266
2019/05/01	0.342
2019/06/01	0.342
2019/07/01	0.342
2019/08/01	0.342
2019/09/01	0.647
2019/10/01	0.647
2019/11/01	0.868
2019/12/01	0.868
2020/01/01	0.962
2020/02/01	0.962
2020/03/01	2.378
2020/04/01	2.433
2020/05/01	2.433
2020/06/01	2.433
```

`freshli` should build and run on any platform that's supported by the .NET Core SDK. It is heavily tested on both macOS and Windows. If you run into problems, please open an issue. The output from above was captured from running in `zsh` on macOS Catalina (10.15.5).


### Building `freshli`

There are multiple ways to build `freshli`. The simplest is directly on the command line by running `dotnet build`. You can also use an IDE for working on `freshli`. Most of the uses JetBrains Rider, but you can also use Visual Studio 2019. If you don't want to use an IDE, then a text editor with good C# support such as Visual Studio Code or Atom also works equally well.

This is what a successful command line build looks like:

```
➜  freshli git:(main) ✗ dotnet build
Microsoft (R) Build Engine version 16.6.0+5ff7b0c9e for .NET Core
Copyright (C) Microsoft Corporation. All rights reserved.

  Determining projects to restore...
  Restored /Users/mscottford/src/corgibytes/freshli/Freshli/Freshli.csproj (in 244 ms).
  Restored /Users/mscottford/src/corgibytes/freshli/Freshli.Test/Freshli.Test.csproj (in 303 ms).
VersionInfo.cs(200,32): warning CS8632: The annotation for nullable reference types should only be used in code within a '#nullable' annotations context. [/Users/mscottford/src/corgibytes/freshli/Freshli/Freshli.csproj]
  Freshli -> /Users/mscottford/src/corgibytes/freshli/Freshli/bin/Debug/netcoreapp3.1/Freshli.dll
  Freshli.Test -> /Users/mscottford/src/corgibytes/freshli/Freshli.Test/bin/Debug/netcoreapp3.1/Freshli.Test.dll
  Archive:  nokotest.zip
    inflating: nokotest/Gemfile
    inflating: nokotest/Gemfile.lock
    inflating: nokotest/.git/config
   extracting: nokotest/.git/objects/0d/8f4f864a22eac5f72153cf1d77fc9791796e6d
   extracting: nokotest/.git/objects/93/e24fec7e2d55e1f2649989a131b1a044008e60
   extracting: nokotest/.git/objects/bb/e94adc863a728d5c63b1293a7d1d81ac437f30
   extracting: nokotest/.git/objects/6e/dae1c2dc746439f567894cf77effc7a8abf97b
   extracting: nokotest/.git/objects/01/7031627f36deb582d69cddd381718be0044b02
   extracting: nokotest/.git/objects/90/2a3082740f83776eec419c59a56e54424fdec5
   extracting: nokotest/.git/objects/b9/803963c64c5c8794334bb667d98c969add6fd0
   extracting: nokotest/.git/objects/b9/d397bcc26e2a820a2e077298f35521b154febd
   extracting: nokotest/.git/objects/c4/a0ab82b5bf0d03d646348bce24527d84d8bfe4
   extracting: nokotest/.git/objects/e1/be34540508cfb94fea222ecdc61a95652068ee
   extracting: nokotest/.git/objects/76/06873e8c521ba79d093029969c2da124ed03d3
   extracting: nokotest/.git/objects/13/963f09081c175c66d20f7dd15d23fedc789ce4
   extracting: nokotest/.git/HEAD
    inflating: nokotest/.git/info/exclude
    inflating: nokotest/.git/logs/HEAD
    inflating: nokotest/.git/logs/refs/heads/master
    inflating: nokotest/.git/description
    inflating: nokotest/.git/hooks/commit-msg.sample
    inflating: nokotest/.git/hooks/pre-rebase.sample
    inflating: nokotest/.git/hooks/pre-commit.sample
    inflating: nokotest/.git/hooks/applypatch-msg.sample
    inflating: nokotest/.git/hooks/prepare-commit-msg.sample
    inflating: nokotest/.git/hooks/post-update.sample
    inflating: nokotest/.git/hooks/pre-applypatch.sample
    inflating: nokotest/.git/hooks/pre-push.sample
    inflating: nokotest/.git/hooks/update.sample
   extracting: nokotest/.git/refs/heads/master
    inflating: nokotest/.git/index
   extracting: nokotest/.git/COMMIT_EDITMSG

Build succeeded.

VersionInfo.cs(200,32): warning CS8632: The annotation for nullable reference types should only be used in code within a '#nullable' annotations context. [/Users/mscottford/src/corgibytes/freshli/Freshli/Freshli.csproj]
    1 Warning(s)
    0 Error(s)

Time Elapsed 00:00:06.31
```

### Running the test suite

Simply running `dotnet test` will kick off the test runner. If you're using an IDE to build `freshli`, such as JetBrains Rider or Visual Studio 2019, then you can use the test runner that's built into the IDE.

Here's an example of a successful test run:

```
➜  freshli git:(main) ✗ dotnet test
VersionInfo.cs(200,32): warning CS8632: The annotation for nullable reference types should only be used in code within a '#nullable' annotations context. [/Users/mscottford/src/corgibytes/freshli/Freshli/Freshli.csproj]
Test run for /Users/mscottford/src/corgibytes/freshli/Freshli.Test/bin/Debug/netcoreapp3.1/Freshli.Test.dll(.NETCoreApp,Version=v3.1)
Microsoft (R) Test Execution Command Line Tool Version 16.6.0
Copyright (c) Microsoft Corporation.  All rights reserved.

Starting test execution, please wait...

A total of 1 test files matched the specified pattern.
2020/06/18 16:26:41.137| INFO|Freshli.Runner:13|Run(/Users/mscottford/src/corgibytes/freshli/Freshli.Test/bin/Debug/netcoreapp3.1/fixtures/ruby/nokotest, 1/1/2020 12:00:00 AM)
2020/06/18 16:26:41.727| INFO|Freshli.Runner:13|Run(https://github.com/feedbin/feedbin, 1/1/2020 12:00:00 AM)
2020/06/18 16:27:44.109| INFO|Freshli.Runner:13|Run(/Users/mscottford/src/corgibytes/freshli/Freshli.Test/bin/Debug/netcoreapp3.1/fixtures/php/large, 1/1/2020 12:00:00 AM)
2020/06/18 16:28:29.944| INFO|Freshli.Runner:13|Run(/Users/mscottford/src/corgibytes/freshli/Freshli.Test/bin/Debug/netcoreapp3.1/fixtures/php/drupal, 1/1/2020 12:00:00 AM)
2020/06/18 16:28:38.347| INFO|Freshli.Runner:13|Run(https://github.com/binux/pyspider, 1/1/2020 12:00:00 AM)
2020/06/18 16:28:50.405| INFO|Freshli.Runner:13|Run(https://github.com/corgibytes/freshli-fixture-ruby-nokotest, 1/1/2020 12:00:00 AM)
2020/06/18 16:28:52.291| INFO|Freshli.Runner:13|Run(https://github.com/thoughtbot/clearance, 6/18/2020 12:00:00 AM)

Test Run Successful.
Total tests: 334
     Passed: 334
 Total time: 2.6251 Minutes
 ```

 The tests currently take longer to run than we would like. We're exploring ways to speed that up.

## Logging

[NLog](https://nlog-project.org/) is being used for logging within the application.

Configuration is in [`Freshli/NLog.config`](Freshli/NLog.config)

The logger is configured to write to the console as well as to:
  * `Freshli/bin/Debug/netcoreapp3.1/freshli.log`
  * `Freshli.Test/bin/Debug/netcoreapp3.1/freshli.log`

## Generating Graphs

### Spreadsheets

The console output is designed to make it easy to copy and paste into a spreadsheet application and take advantage of the charting libraries that are available there. Additional output formats will be added in the future.

### Jupyter Notebook

If you want to generate graphs in a Jupyter Notebook, there is some additional setup that you need to follow.

1. Install Jupyter-Lab or nteract
1. Install dontet/interactive

A [sample notebook](https://github.com/corgibytes/freshli/blob/main/Sample.ipynb) can be found in this repository.

Before working with `freshli` in a Jupyter Notebook you have to package it as a NuGet package by running `dotnet pack`.

Once that's done, you'll need to import and require `freshli` as a NuGet package by running the following.

```
#i nuget:/Users/mscottford/src/corgibytes/freshli/Freshli/bin/Debug
#r nuget:freshli,1.0.0-dev
```

You'll need to change the path above to match the absolute path where the `.nupkg` file can be found.

The following script will allow you to create simple line graphs to plot a project or multiple projects.

```csharp

using XPlot.Plotly;

using Freshli;
using Freshli.Languages.Ruby;
using Freshli.Languages.Php;
using Freshli.Languages.Python;

ManifestFinder.Register<RubyBundlerManifestFinder>();
ManifestFinder.Register<PhpComposerManifestFinder>();
ManifestFinder.Register<PipRequirementsTxtManifestFinder>();

FileHistoryFinder.Register<GitFileHistoryFinder>();

PlotlyChart CreateLineGraphFor(Dictionary<string, IList<MetricsResult>> projects) {
    var lineSeries = projects.Select(p => new Scattergl {
        name = p.Key,
        x = p.Value.Select(r => r.Date),
        y = p.Value.Select(r => r.LibYear.Total)
    });


    var chart = Chart.Plot(lineSeries.ToArray());
    chart.WithTitle("LibYear over time");
    return chart;
}
```

You can then generate a graph by running:

```csharp
var runner = new Runner();
var projects = new Dictionary<string, IList<MetrictsResult>>();
projects["example"] = runner.Run("https://github.com/corgibytes/freshli-fixture-ruby-nokotest");
display(CreateLineGraphFor(projects))
```
