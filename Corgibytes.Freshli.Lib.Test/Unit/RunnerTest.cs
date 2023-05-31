using System.Collections.Generic;
using Xunit;

namespace Corgibytes.Freshli.Lib.Test.Unit;

public class RunnerTest
{
    [Fact]
    public void RunCSharp()
    {
        string fixturePath = Fixtures.Path(
            "csharp"
        );
        Runner runner = new();
        IList<ScanResult> scanResults = runner.Run(fixturePath);
        Assert.Equal(2, scanResults.Count);
        Assert.Contains(scanResults, result => result.Filename.Equals("csproj/Project.csproj"));
        Assert.Contains(scanResults, result => result.Filename.Equals("config/packages.config"));
    }

}
