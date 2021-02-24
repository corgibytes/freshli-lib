[![.NET Core](https://github.com/corgibytes/freshli/workflows/.NET%20Core/badge.svg)](https://github.com/corgibytes/freshli/actions?query=workflow%3A%22.NET+Core%22)
[![Docker Image CI](https://github.com/corgibytes/freshli/workflows/Docker%20Image%20CI/badge.svg)](https://github.com/corgibytes/freshli/actions?query=workflow%3A%22Docker+Image+CI%22)
[![EditorConfig Lint](https://github.com/corgibytes/freshli/workflows/EditorConfig%20Lint/badge.svg)](https://github.com/corgibytes/freshli/actions?query=workflow%3A%22EditorConfig+Lint%22)
[![Maintainability](https://api.codeclimate.com/v1/badges/d808370d214bbda62e58/maintainability)](https://codeclimate.com/repos/601c1f18f9414200c900094b/maintainability)
[![Test Coverage](https://api.codeclimate.com/v1/badges/d808370d214bbda62e58/test_coverage)](https://codeclimate.com/repos/601c1f18f9414200c900094b/test_coverage)

# freshli
A tool for collecting historical metrics about a project's dependencies

The `freshli` tool captures historical metrics about a project's dependencies. In it's current form, the only metric that it computes is [libyear](https://libyear.com/).

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

## Using Freshli in a Project

To use Freshli in a project, you have two options:

### Stable and Beta Releases

Stable and beta releases are found at https://www.nuget.org/packages/Freshli/ and can be added with the [`dotnet add package`](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-add-package) command:

```
dotnet add package Freshli.Core
```

### Alpha Releases

To use an alpha release, you'll need to [set up a GitHub personal access token](https://docs.github.com/en/github/authenticating-to-github/creating-a-personal-access-token) and then create a `nuget.config` file in your project root with the following contents:

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

## Contributing to `freshli`

More information can be found at the [Contributing Guide]()

