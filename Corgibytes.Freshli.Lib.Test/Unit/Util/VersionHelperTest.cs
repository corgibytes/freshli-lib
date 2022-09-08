using Corgibytes.Freshli.Lib.Util;
using Xunit;

namespace Corgibytes.Freshli.Lib.Test.Unit.Util
{
    public class VersionHelperTest
    {

        [Theory]
        [InlineData(1L, 1L, 0)]
        [InlineData(100L, 1L, 1)]
        [InlineData(1L, 100L, -1)]
        [InlineData(null, null, 0)]
        [InlineData(1L, null, 1)]
        [InlineData(null, 1L, -1)]

        public void NumericValuesAreComparedCorrectly(
          long? firstValue,
          long? secondValue,
          int comparison
        )
        {
            Assert.Equal(
              comparison,
              VersionHelper.CompareNumericValues(firstValue, secondValue)
            );
        }
    }
}
