using Freshli.Util;
using Xunit;

namespace Freshli.Test.Unit.Util {
  public class VersionHelperTest {

    [Theory]
    [InlineData(1, 1, 0)]
    [InlineData(100, 1, 1)]
    [InlineData(1, 100, -1)]
    [InlineData(null, null, 0)]
    [InlineData(1, null, 1)]
    [InlineData(null, 1, -1)]

    public void NumericValuesAreComparedCorrectly(
      long? firstValue,
      long? secondValue,
      int comparison
    ) {
      Assert.Equal(
        comparison,
        VersionHelper.CompareNumericValues(firstValue, secondValue)
      );
    }
  }
}
