# freshli CLI

## Getting started with `freshli` CLI

### Running `freshli`

Right now to run `freshli` from the command line, you need to have the .NET Core SDK installed.

Once you do, you can run use `dotnet run --project Freshli-CLI/Freshli-CLI.csproj -- <url>` to run the project.

Here's an example:

```
➜  freshli git:(main) ✗ dotnet run --project Freshli-CLI/Freshli-CLI.csproj -- http://github.com/corgibytes/freshli-fixture-ruby-nokotest
output
```

`freshli-cli` and `freshli` should build and run on any platform that's supported by the .NET Core SDK. It is heavily tested on both macOS and Windows. If you run into problems, please open an issue. The output from above was captured from running in `zsh` on macOS Catalina (10.15.5).

### Building `freshli-cli`

There are multiple ways to build `freshli`. The simplest is directly on the command line by running `dotnet build`.

You can also use an IDE for working on `freshli`. Most of the project's developers use JetBrains Rider, but you can also use Visual Studio 2019. If you don't want to use an IDE, then a text editor with good C# support such as Visual Studio Code or Atom also works equally well.

This is what a successful command line build looks like:

```
➜  freshli git:(main) ✗ dotnet build
output
```

### Running the test suite

Simply running `dotnet test` will kick off the test runner. If you're using an IDE to build `freshli`, such as JetBrains Rider or Visual Studio 2019, then you can use the test runner that's built into the IDE.

Here's an example of a successful test run:

```
➜  freshli git:(main) ✗ dotnet test
output here
```

The tests currently take longer to run than we would like. We're exploring ways to speed that up. You can run a subset of tests by including the `--filter` flag, e.g. `dotnet test --filter ComputeAsOf`.
