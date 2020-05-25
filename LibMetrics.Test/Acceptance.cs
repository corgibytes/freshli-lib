using System;
using System.Collections.Generic;
using System.IO;
using LibMetrics.Languages.Php;
using LibMetrics.Languages.Ruby;
using Xunit;

namespace LibMetrics.Test
{
  public class Acceptance
  {
    [Fact]
    public void RubyGemsWithGitHistory()
    {
      ManifestFinder.Register<RubyBundlerManifestFinder>();
      FileHistoryFinder.Register<GitFileHistoryFinder>();

      var runner = new Runner();

      var rubyFixturePath = Fixtures.Path("ruby", "nokotest");
      var results = runner.Run(rubyFixturePath);

      AssertNokoFixtureRubyGemsResults(results);
    }

    [Fact]
    public void RubyGemsWithHistoryViaGitHub() {
      ManifestFinder.Register<RubyBundlerManifestFinder>();
      FileHistoryFinder.Register<GitFileHistoryFinder>();

      var runner = new Runner();

      var repoUrl =
        "https://github.com/corgibytes/libmetrics-fixture-ruby-nokotest";
      var results = runner.Run(repoUrl);

      AssertNokoFixtureRubyGemsResults(results);
    }

    [Fact]
    public void RubyGemsFeedbinHistoryViaGitHub()
    {
      ManifestFinder.Register<RubyBundlerManifestFinder>();
      FileHistoryFinder.Register<GitFileHistoryFinder>();

      var runner = new Runner();

      var repoUrl = "https://github.com/feedbin/feedbin";
      var results = runner.Run(repoUrl);

      var output = new StringWriter();

      foreach (var resultSet in results) {
        if (resultSet.Date <= new DateTime(2020, 01, 01)) {
          output.WriteLine(
            $"{resultSet.Date.ToString("yyyy/MM/dd")}: " +
            $"{resultSet.LibYear.Total.ToString("F3")}"
          );
        }
      }

      var expected = new StringWriter();
      expected.WriteLine("2013/09/01: 7.444");
      expected.WriteLine("2013/10/01: 4.649");
      expected.WriteLine("2013/11/01: 8.389");
      expected.WriteLine("2013/12/01: 20.879");
      expected.WriteLine("2014/01/01: 11.762");
      expected.WriteLine("2014/02/01: 10.986");
      expected.WriteLine("2014/03/01: 17.814");
      expected.WriteLine("2014/04/01: 10.940");
      expected.WriteLine("2014/05/01: 21.677");
      expected.WriteLine("2014/06/01: 16.858");
      expected.WriteLine("2014/07/01: 38.778");
      expected.WriteLine("2014/08/01: 26.989");
      expected.WriteLine("2014/09/01: 32.118");
      expected.WriteLine("2014/10/01: 36.019");
      expected.WriteLine("2014/11/01: 41.945");
      expected.WriteLine("2014/12/01: 25.701");
      expected.WriteLine("2015/01/01: 24.353");
      expected.WriteLine("2015/02/01: 38.819");
      expected.WriteLine("2015/03/01: 48.411");
      expected.WriteLine("2015/04/01: 56.499");
      expected.WriteLine("2015/05/01: 65.767");
      expected.WriteLine("2015/06/01: 68.696");
      expected.WriteLine("2015/07/01: 18.060");
      expected.WriteLine("2015/08/01: 25.814");
      expected.WriteLine("2015/09/01: 13.973");
      expected.WriteLine("2015/10/01: 22.030");
      expected.WriteLine("2015/11/01: 30.789");
      expected.WriteLine("2015/12/01: 18.279");
      expected.WriteLine("2016/01/01: 25.304");
      expected.WriteLine("2016/02/01: 20.351");
      expected.WriteLine("2016/03/01: 32.658");
      expected.WriteLine("2016/04/01: 26.863");
      expected.WriteLine("2016/05/01: 37.296");
      expected.WriteLine("2016/06/01: 23.279");
      expected.WriteLine("2016/07/01: 22.022");
      expected.WriteLine("2016/08/01: 34.249");
      expected.WriteLine("2016/09/01: 17.077");
      expected.WriteLine("2016/10/01: 22.512");
      expected.WriteLine("2016/11/01: 26.742");
      expected.WriteLine("2016/12/01: 43.586");
      expected.WriteLine("2017/01/01: 25.370");
      expected.WriteLine("2017/02/01: 37.532");
      expected.WriteLine("2017/03/01: 51.334");
      expected.WriteLine("2017/04/01: 61.827");
      expected.WriteLine("2017/05/01: 72.652");
      expected.WriteLine("2017/06/01: 11.934");
      expected.WriteLine("2017/07/01: 21.200");
      expected.WriteLine("2017/08/01: 16.279");
      expected.WriteLine("2017/09/01: 15.085");
      expected.WriteLine("2017/10/01: 24.600");
      expected.WriteLine("2017/11/01: 29.740");
      expected.WriteLine("2017/12/01: 25.940");
      expected.WriteLine("2018/01/01: 28.885");
      expected.WriteLine("2018/02/01: 47.005");
      expected.WriteLine("2018/03/01: 33.636");
      expected.WriteLine("2018/04/01: 45.693");
      expected.WriteLine("2018/05/01: 54.318");
      expected.WriteLine("2018/06/01: 61.781");
      expected.WriteLine("2018/07/01: 36.395");
      expected.WriteLine("2018/08/01: 24.477");
      expected.WriteLine("2018/09/01: 28.427");
      expected.WriteLine("2018/10/01: 32.351");
      expected.WriteLine("2018/11/01: 43.534");
      expected.WriteLine("2018/12/01: 61.312");
      expected.WriteLine("2019/01/01: 38.600");
      expected.WriteLine("2019/02/01: 43.090");
      expected.WriteLine("2019/03/01: 33.748");
      expected.WriteLine("2019/04/01: 48.912");
      expected.WriteLine("2019/05/01: 61.841");
      expected.WriteLine("2019/06/01: 39.671");
      expected.WriteLine("2019/07/01: 37.249");
      expected.WriteLine("2019/08/01: 48.112");
      expected.WriteLine("2019/09/01: 44.671");
      expected.WriteLine("2019/10/01: 55.962");
      expected.WriteLine("2019/11/01: 72.252");
      expected.WriteLine("2019/12/01: 75.430");
      expected.WriteLine("2020/01/01: 34.868");

      Assert.Equal(expected.ToString(), output.ToString());
    }

    private static void AssertNokoFixtureRubyGemsResults(IList<MetricsResult> results) {
      // nokotest contains a git repository with the following history:
      // Gemfile v1 created on 2017/01/01 references nokogiri 1.7.0
      // Gemfile v2 created on 2018/01/01 references nokogiri 1.8.1
      // Gemfile v3 created on 2019/01/01 references nokogiri 1.9.1

      var output = new StringWriter();

      foreach (var resultSet in results) {
          if (resultSet.Date <= new DateTime(2020, 01, 01)) {
              output.WriteLine(
                  $"{resultSet.Date.ToString("yyyy/MM/dd")}: " +
                  $"{resultSet.LibYear.Total.ToString("F3")}"
              );
          }
      }

      var expected = new StringWriter();
      expected.WriteLine("2017/01/01: 0.000");
      expected.WriteLine("2017/02/01: 0.022");
      expected.WriteLine("2017/03/01: 0.022");
      expected.WriteLine("2017/04/01: 0.227");
      expected.WriteLine("2017/05/01: 0.227");
      expected.WriteLine("2017/06/01: 0.364");
      expected.WriteLine("2017/07/01: 1.852");
      expected.WriteLine("2017/08/01: 1.852");
      expected.WriteLine("2017/09/01: 1.852");
      expected.WriteLine("2017/10/01: 2.416");
      expected.WriteLine("2017/11/01: 2.416");
      expected.WriteLine("2017/12/01: 2.416");
      expected.WriteLine("2018/01/01: 0.000");
      expected.WriteLine("2018/02/01: 0.362");
      expected.WriteLine("2018/03/01: 0.362");
      expected.WriteLine("2018/04/01: 0.362");
      expected.WriteLine("2018/05/01: 0.362");
      expected.WriteLine("2018/06/01: 0.362");
      expected.WriteLine("2018/07/01: 0.740");
      expected.WriteLine("2018/08/01: 0.789");
      expected.WriteLine("2018/09/01: 0.789");
      expected.WriteLine("2018/10/01: 0.789");
      expected.WriteLine("2018/11/01: 1.044");
      expected.WriteLine("2018/12/01: 1.044");
      expected.WriteLine("2019/01/01: 0.000");
      expected.WriteLine("2019/02/01: 0.071");
      expected.WriteLine("2019/03/01: 0.071");
      expected.WriteLine("2019/04/01: 0.266");
      expected.WriteLine("2019/05/01: 0.342");
      expected.WriteLine("2019/06/01: 0.342");
      expected.WriteLine("2019/07/01: 0.342");
      expected.WriteLine("2019/08/01: 0.342");
      expected.WriteLine("2019/09/01: 0.647");
      expected.WriteLine("2019/10/01: 0.647");
      expected.WriteLine("2019/11/01: 0.868");
      expected.WriteLine("2019/12/01: 0.868");
      expected.WriteLine("2020/01/01: 0.962");

      Assert.Equal(expected.ToString(), output.ToString());
    }

    [Fact]
    public void ComposerWithoutGitHistory()
    {
      ManifestFinder.Register<PhpComposerManifestFinder>();

      var runner = new Runner();

      var phpFixturePath = Fixtures.Path("php", "large");
      var results = runner.Run(
        phpFixturePath,
        asOf: new DateTime(2020, 01, 01));

      var output = new StringWriter();

      foreach (var resultSet in results)
      {
        output.WriteLine($"{resultSet.Date.ToString("yyyy/MM/dd")}: " +
                         $"{resultSet.LibYear.Total.ToString("F3")}");
      }

      var expected = new StringWriter();
      expected.WriteLine("2020/01/01: 6.126");

      Assert.Equal(expected.ToString(), output.ToString());
    }

    [Fact]
    public void DrupalComposerWithoutGitHistory()
    {
      ManifestFinder.Register<PhpComposerManifestFinder>();

      var runner = new Runner();

      var phpFixturePath = Fixtures.Path("php", "drupal");
      var results = runner.Run(
        phpFixturePath,
        asOf: new DateTime(2020, 01, 01));

      var output = new StringWriter();

      foreach (var resultSet in results)
      {
        output.WriteLine($"{resultSet.Date.ToString("yyyy/MM/dd")}: " +
                         $"{resultSet.LibYear.Total.ToString("F3")}");
      }

      var expected = new StringWriter();
      expected.WriteLine("2020/01/01: 3.214");

      Assert.Equal(expected.ToString(), output.ToString());
    }
  }
}
