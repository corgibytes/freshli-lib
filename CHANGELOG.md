## v0.4.0


As part of this release we had [19 issues](https://github.com/corgibytes/freshli-lib/milestone/5?closed=1) closed.
Goals for this milestone:

- Common Freshli dependency interface.
- Support parsing multiple manifests but specifically focusing on .NET for this release.
- Beta version of NuGet package.
- Fail build if metrics like CodeClimate, Coverage, etc, are worse then previous build.

__Bug__

- [__#290__](https://github.com/corgibytes/freshli-lib/pull/290) Fix changelog line endings for git auto commit

__DevOps__

- [__#293__](https://github.com/corgibytes/freshli-lib/issues/293) Release notes CI fails for Dependabot PRs
- [__#287__](https://github.com/corgibytes/freshli-lib/pull/287) Solve `Some checks haven't completed yet` issue caused by use of `git-auto-commit-action`
- [__#283__](https://github.com/corgibytes/freshli-lib/issues/283) Remove old Dependabot config
- [__#277__](https://github.com/corgibytes/freshli-lib/pull/277) Add nuspec information
- [__#268__](https://github.com/corgibytes/freshli-lib/issues/268) Version not correctly incremented when merging tag from release to main
- [__#261__](https://github.com/corgibytes/freshli-lib/pull/261) Sets up minimal rename for NuGet push
- [__#253__](https://github.com/corgibytes/freshli-lib/issues/253) Fix multiple CodeClimate Freshli projects

__Dependencies__

- [__#306__](https://github.com/corgibytes/freshli-lib/pull/306) Bump HtmlAgilityPack from 1.11.32 to 1.11.33
- [__#302__](https://github.com/corgibytes/freshli-lib/pull/302) Bump DiffEngine from 6.6.1 to 6.7.0
- [__#300__](https://github.com/corgibytes/freshli-lib/pull/300) Bump Microsoft.NET.Test.Sdk from 16.9.1 to 16.9.4
- [__#297__](https://github.com/corgibytes/freshli-lib/pull/297) Bump NLog from 4.7.8 to 4.7.9
- [__#296__](https://github.com/corgibytes/freshli-lib/pull/296) Bump Elasticsearch.Net from 7.11.1 to 7.12.0
- [__#295__](https://github.com/corgibytes/freshli-lib/pull/295) Bump HtmlAgilityPack from 1.11.31 to 1.11.32
- [__#292__](https://github.com/corgibytes/freshli-lib/pull/292) Bump ApprovalTests from 5.4.6 to 5.4.7
- [__#280__](https://github.com/corgibytes/freshli-lib/pull/280) Bump DiffEngine from 6.5.7 to 6.5.9
- [__#260__](https://github.com/corgibytes/freshli-lib/pull/260) Bump DotNetEnv from 2.1.0 to 2.1.1

__Enhancements__

- [__#266__](https://github.com/corgibytes/freshli-lib/issues/266) Labels not included in release notes
- [__#259__](https://github.com/corgibytes/freshli-lib/issues/259) Rename this repo and/or C# namespace


## v0.3.0


As part of this release we had [8 issues](https://github.com/corgibytes/freshli-lib/milestone/4?closed=1) closed.

Goals for this milestone:

- Alpha NuGet package published to GitHub Packages
- Automate calculation of version number and change log
- Clean-up of the ReadMe.
- Basic support for Ruby, .NET, Python, Perl, and PHP.

__Bug__

- [__#258__](https://github.com/corgibytes/freshli-lib/pull/258) Ignores PHP dependencies for test projects

__DevOps__

- [__#241__](https://github.com/corgibytes/freshli-lib/issues/241) Auto generate change log and GitHub releases

__Documentation__

- [__#242__](https://github.com/corgibytes/freshli-lib/issues/242) Clean-up ReadMe for v0.3.0 release.
- [__#225__](https://github.com/corgibytes/freshli-lib/issues/225) Update Jupyter Notebook Documentation

__Enhancements__

- [__#239__](https://github.com/corgibytes/freshli-lib/pull/239) Deploys Alpha Packages to Github Packages
- [__#238__](https://github.com/corgibytes/freshli-lib/pull/238) Sets VSCode container development
- [__#228__](https://github.com/corgibytes/freshli-lib/issues/228) Auto-magically calculate version number
- [__#4__](https://github.com/corgibytes/freshli-lib/issues/4) Add support for NuGet (specific to using Freshli.Core)


