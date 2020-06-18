# freshli
A CLI tool for collecting historical metrics about a project's dependencies

The `freshli` command line tool captures historical metrics about a project's dependencies. In it's current form, the only metric that it computes is [libyear](https://libyear.com/).

For each language that the tool supports, the libyear metric is computed for each month in the past where dependency information is available.

## Supported Tools

### Languages and Dependency Frameworks

* Ruby
  * bundler - reads information from `Gemfile.lock`
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

`freshli` should build and run on any platform that's supported by the .NET Core SDK. It is heavily tested on both macOS and Windows. If you run into problems, please open an issue.


### Building `freshli`

There are multiple ways to build `freshli`. The simplest is directly on the command line by running `dotnet build`. You can also use an IDE for working on `freshli`. Most of the uses JetBrains Rider, but you can also use Visual Studio 2019. If you don't want to use an IDE, then a text editor with good C# support such as Visual Studio Code or Atom also works equally well.

### Running the test suite

Simply running `dotnet test` will kick off the test runner. If you're using an IDE to build `freshli`, such as JetBrains Rider or Visual Studio 2019, then you can use the test runner that's built into the IDE.

## Logging

[NLog](https://nlog-project.org/) is being used for logging within the application.

Configuration is in [`Freshli/NLog.config`](Freshli/NLog.config)

The logger is configured to write to the console as well as to:
  * `Freshli/bin/Debug/netcoreapp3.1/freshli.log`
  * `Freshli.Test/bin/Debug/netcoreapp3.1/freshli.log`
