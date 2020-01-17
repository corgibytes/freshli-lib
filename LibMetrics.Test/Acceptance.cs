using System;
using System.IO;
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

      // nokotest contains a git repository with the following history:
      // Gemfile v1 created on 2017/01/01 references nokogiri 1.7.0
      // Gemfile v2 created on 2018/01/01 references nokogiri 1.8.1
      // Gemfile v3 created on 2019/01/01 references nokogiri 1.9.1

      var rubyFixturePath = Fixtures.Path("ruby", "nokotest");
      var results = runner.Run(rubyFixturePath);

      var output = new StringWriter();

      foreach (var resultSet in results)
      {
        if (resultSet.Date <= new DateTime(2020, 01, 01))
        {
          output.WriteLine($"{resultSet.Date.ToString("yyyy/MM/dd")}: " +
                           $"{resultSet.LibYear.Total.ToString("F3")}");
        }
      }

      var expected = new StringWriter();
      expected.WriteLine("2017/01/01: 0.000");
      expected.WriteLine("2017/02/01: 0.022");
      expected.WriteLine("2017/03/01: 0.022");
      expected.WriteLine("2017/04/01: 0.227");
      expected.WriteLine("2017/05/01: 0.227");
      expected.WriteLine("2017/06/01: 0.366");
      expected.WriteLine("2017/07/01: 1.850");
      expected.WriteLine("2017/08/01: 1.850");
      expected.WriteLine("2017/09/01: 1.850");
      expected.WriteLine("2017/10/01: 2.418");
      expected.WriteLine("2017/11/01: 2.418");
      expected.WriteLine("2017/12/01: 2.418");
      expected.WriteLine("2018/01/01: 0.000");
      expected.WriteLine("2018/02/01: 0.361");
      expected.WriteLine("2018/03/01: 0.361");
      expected.WriteLine("2018/04/01: 0.361");
      expected.WriteLine("2018/05/01: 0.361");
      expected.WriteLine("2018/06/01: 0.361");
      expected.WriteLine("2018/07/01: 0.740");
      expected.WriteLine("2018/08/01: 0.787");
      expected.WriteLine("2018/09/01: 0.787");
      expected.WriteLine("2018/10/01: 0.787");
      expected.WriteLine("2018/11/01: 1.042");
      expected.WriteLine("2018/12/01: 1.042");
      expected.WriteLine("2019/01/01: 0.000");
      expected.WriteLine("2019/02/01: 0.071");
      expected.WriteLine("2019/03/01: 0.071");
      expected.WriteLine("2019/04/01: 0.267");
      expected.WriteLine("2019/05/01: 0.344");
      expected.WriteLine("2019/06/01: 0.344");
      expected.WriteLine("2019/07/01: 0.344");
      expected.WriteLine("2019/08/01: 0.344");
      expected.WriteLine("2019/09/01: 0.648");
      expected.WriteLine("2019/10/01: 0.648");
      expected.WriteLine("2019/11/01: 0.870");
      expected.WriteLine("2019/12/01: 0.870");
      expected.WriteLine("2020/01/01: 0.963");

      Assert.Equal(expected.ToString(), output.ToString());
    }

    [Fact(Skip = "wip")]
    public void ComposerWithoutGitHistory()
    {
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
      expected.WriteLine("2020/01/01: 0.000");

      Assert.Equal(expected.ToString(), output.ToString());
    }
  }
}
