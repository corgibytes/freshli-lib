# libmetrics
A CLI tool for collecting historical metrics about a project's dependencies

The `libmetrics` command line tool captures historical metrics about a project's dependencies. In it's current form, the only metric that it computes is [libyear](https://libyear.com/).

For each language that the tool supports, the libyear metric is computed for each month that the

## Supported Tools

### Languages and Dependency Frameworks

* Ruby
  * bundler - reads information from `Gemfile.lock`
* PHP
  * composer - reads information from `composer.json` and `composer.lock`

### Source Code Repositories

* Git