[![.NET Core](https://github.com/corgibytes/freshli-lib/workflows/.NET%20Core/badge.svg)](https://github.com/corgibytes/freshli/actions?query=workflow%3A%22.NET+Core%22)
[![Docker Image CI](https://github.com/corgibytes/freshli-lib/workflows/Docker%20Image%20CI/badge.svg)](https://github.com/corgibytes/freshli/actions?query=workflow%3A%22Docker+Image+CI%22)
[![EditorConfig Lint](https://github.com/corgibytes/freshli-lib/workflows/EditorConfig%20Lint/badge.svg)](https://github.com/corgibytes/freshli/actions?query=workflow%3A%22EditorConfig+Lint%22)
[![Maintainability](https://api.codeclimate.com/v1/badges/4d7b974eedea679e6b03/maintainability)](https://codeclimate.com/github/corgibytes/freshli-lib/maintainability)
[![Test Coverage](https://api.codeclimate.com/v1/badges/4d7b974eedea679e6b03/test_coverage)](https://codeclimate.com/github/corgibytes/freshli-lib/test_coverage)

# Freshli-Lib
A library for computing historical metrics about a project's dependencies.

If you are looking for a completed application please see the [Freshli CLI](https://github.com/corgibytes/freshli-cli) or [Freshli Website](https://freshli.io/).

## Getting Started
Freshli-Lib is a available as a NuGet [package](https://www.nuget.org/packages/Corgibytes.Freshli.Lib/).  You can install it using your favoriate IDE GUI or by running the following command:

```
dotnet add package Corgibytes.Freshli.Lib
```

An example of using Freshli-Lib:

```csharp
using Corgibytes.Freshli.Lib;

// The runner takes the path to your repository.
var runner = new Runner();
var results = runner.Run(repositoryUrl);

// You can view both the manifest file name and a collection of metric results
Console.WriteLine(results.Filename);
Console.WriteLine(results.MetricsResults);

// You can also output the entire ScanResult
Console.WriteLine(results[0].ToString());
```

### Alpah Packages

If you like living on the edge you can find alpha versions of Freshli Lib [here](https://github.com/corgibytes/freshli-lib/packages/667787/versions).  You will need to [set up a GitHub personal access token](https://docs.github.com/en/github/authenticating-to-github/creating-a-personal-access-token) and then create a `nuget.config` file in your project root with the following contents:

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

## Supported Dependency Managers

The dependency managers that Freshli supports are listed below along with the manifest files it can parse.  The manifest file is the file that lists what dependencies are required by the project and has changed over time for some dependency managers, like NuGet.

| Dependency Manager | Language/Framework | Manifest Files Format |
|--------------------|--------------------|-----------------------|
| [Bundler](https://bundler.io/) | [Ruby](https://www.ruby-lang.org), [Ruby on Rails](https://rubyonrails.org/) | Gemfile.lock |
| [Carton](https://metacpan.org/pod/Carton) | [Perl](https://www.perl.org/) | cpanfile |
| [Composer](https://getcomposer.org/) | [PHP](https://www.php.net/) | composer.json, composer.lock |
| [Pip](https://pypi.org/project/pip/) | [Python](https://www.python.org/) | requirements.txt |
| [NuGet](https://www.nuget.org/) | [C#](https://docs.microsoft.com/en-us/dotnet/csharp/) | *.csproj |

Please let us know what other dependency managers and/or manifest files you would like use to support via the contact information in the [Contributing](#contributing) section.

## Supported Source Control Tools

Freshli reads source code repository history to access previous version of each dependency manifest.  Currently Freshli only supports [Git](https://git-scm.com/) but if you would like us to add more let us know via the contact informatino in the [Contributing](#contributing) section.

## Contributing to Freshli-Lib

If you have any questions, notice a bug, or have a suggestion/enhancment please let us know by opening a [issue](https://github.com/corgibytes/freshli-lib/issues) or [pull request](https://github.com/corgibytes/freshli-lib/pulls).  More information can be found at the [Contributing Guide](CONTRIBUTING.md)

