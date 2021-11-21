using System;
using System.Runtime.InteropServices;
using Xunit;

namespace Corgibytes.Freshli.Lib.Test.Unit
{
    public class LibYearCalculatorTest
    {
        [Theory]
        [InlineData(
            new object[] { "1.1.0", 2017, 04, 07 },
            new object[] { "1.1.3", 2019, 06, 13 },
            2.18,
            2
        )]
        [InlineData(
            new object[] { "v3.6.0", 2018, 02, 08 },
            new object[] { "v4.0.0", 2019, 08, 28 },
            1.55,
            2
        )]
        [InlineData(
            new object[] { "v1.1.4", 2014, 09, 14 },
            new object[] { "v1.6.8", 2019, 10, 30 },
            5.13,
            2
        )]
        [InlineData(
            new object[] { "1.7.0", 2016, 12, 27 },
            new object[] { "1.7.1", 2017, 03, 20 },
            0.227,
            3
        )]
        public void Compute(object[] oldVersion, object[] newVersion, double expectedValue, int precision)
        {
            var calculator = new LibYearCalculator(null, null, false);
            var actualValue = calculator.Compute(BuildVersion(oldVersion), BuildVersion(newVersion));
            Assert.Equal(expectedValue, actualValue, precision);
        }

        private SemVerVersionInfo BuildVersion(object[] parts)
        {
            var rawVersion = (string)parts[0];
            var year = (int)parts[1];
            var month = (int)parts[2];
            var day = (int)parts[3];
            var date = new DateTimeOffset(year, month, day, 00, 00, 00, TimeSpan.Zero);
            return new(rawVersion, date);
        }
    }
}
