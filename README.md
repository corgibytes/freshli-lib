[![.NET Core](https://github.com/corgibytes/freshli/workflows/.NET%20Core/badge.svg)](https://github.com/corgibytes/freshli/actions?query=workflow%3A%22.NET+Core%22)
[![Docker Image CI](https://github.com/corgibytes/freshli/workflows/Docker%20Image%20CI/badge.svg)](https://github.com/corgibytes/freshli/actions?query=workflow%3A%22Docker+Image+CI%22)
[![EditorConfig Lint](https://github.com/corgibytes/freshli/workflows/EditorConfig%20Lint/badge.svg)](https://github.com/corgibytes/freshli/actions?query=workflow%3A%22EditorConfig+Lint%22)
[![Maintainability](https://api.codeclimate.com/v1/badges/4d7b974eedea679e6b03/maintainability)](https://codeclimate.com/github/corgibytes/freshli-lib/maintainability)
[![Test Coverage](https://api.codeclimate.com/v1/badges/4d7b974eedea679e6b03/test_coverage)](https://codeclimate.com/github/corgibytes/freshli-lib/test_coverage)

# Freshli-Lib
A tool for collecting historical metrics about a project's dependencies

The `Freshli` tool captures historical metrics about a project's dependencies. In it's current form, the only metric that it computes is [libyear](https://libyear.com/).

For each language that the tool supports, the libyear metric is computed for each month in the past where dependency information is available.

## Supported Tools

### Languages and Dependency Frameworks

Freshli reads dependency information from special files called "dependency manifests". Each language community has a different format and some lanugage communities have multiple ones. These are the ones that we support. If you don't see one that's important to you, please create an issue for us to add support for it.

* Ruby
  * bundler - reads information from `Gemfile.lock`
* Perl
  * carton - reads information from `cpanfile`
* PHP
  * composer - reads information from `composer.json` and `composer.lock`
* Python
  * pip - reads information from `requirements.txt`
* .NET
  * NuGet - reads information from `*.csproj`

### Source Code Repositories

Freshli reads source code repository history to access previous version of each dependency manifest. These are the source code repositories that it currently works with. If you don't see your favorite, create an issue for us to add support for it.

* Git

## Using Freshli-Lib in a Project

To use Freshli in a project, you have two options:

### Beta & Alpha Releases

To use a beta or alpha release, you'll need to [set up a GitHub personal access token](https://docs.github.com/en/github/authenticating-to-github/creating-a-personal-access-token) and then create a `nuget.config` file in your project root with the following contents:

```
<?xml version="1.0" encoding="utf-8"?>
<configuration>
    <packageSources>
        <add key="GithubPackages" value="https://nuget.pkg.github.com/corgibytes/index.json" />
    </packageSources>
    <packageSourceCredentials>
        <GithubPackages>
            <add key="Username" value="GITHUB_USERNAME" />
            <add key="ClearTextPassword" value="PERSONAL_ACCESS_TOKEN" />
        </GithubPackages>
    </packageSourceCredentials>
</configuration>
```

You can then [view the listing of alpha packages](https://github.com/orgs/corgibytes/packages?repo_name=freshli), and add the desired alpha package:

```
dotnet add package Freshli -v 0.3.0-alpha0030
```

## Using freshli-lib in your project

As an example on how to use the Freshli library in your source code:

```

var repositoryUrl = YOUR_REPO_URL_HERE;

// pass in the repository URL into the runner to return a collection of ScanResult objects
var results = runner.Run(repositoryUrl);

// you can view both the manifest file name and a collection of metric results
Console.WriteLine(results.Filename);
Console.WriteLine(results.MetricsResults);

// you can also output the entire ScanResult
Console.WriteLine(results[0].ToString());
```

## Contributing to `freshli`

More information can be found at the [Contributing Guide](CONTRIBUTING.md)

Bump
