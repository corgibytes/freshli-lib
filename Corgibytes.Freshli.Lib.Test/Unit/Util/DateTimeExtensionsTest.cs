using System;
using System.Collections.Generic;
using System.Reflection;
using Corgibytes.Freshli.Lib.Util;
using Corgibytes.Xunit.Extensions;
using Xunit;

namespace Corgibytes.Freshli.Lib.Test.Unit.Util
{
    public class TestDateTimeOffsetExtensions
    {
        public class ToEndOfDay: DateModificationExtensionMethodTestFixture<ToEndOfDay>
        {
            public override DateTimeOffset InvokeMethod(DateTimeOffset value) => value.ToEndOfDay();

            public override TheoryData<DateTimeOffset, DateTimeOffset, int> DataForTestingExtensionMethod => new()
            {
                {
                    BuildDateTimeOffset(new[] { 2020, 1, 5, 23, 59, 59, 999, 9998 }),
                    BuildDateTimeOffset(new[] { 2020, 1, 6, 23, 59, 59, 999, 9999 }),
                    -1
                },
                {
                    BuildDateTimeOffset(new[] { 2020, 1, 5, 23, 59, 59, 999, 9999 }),
                    BuildDateTimeOffset(new[] { 2020, 1, 6, 23, 59, 59, 999, 9999 }),
                    -1
                },
                {
                    BuildDateTimeOffset(new[] { 2020, 1, 6, 0, 0, 0, 0, 0 }),
                    BuildDateTimeOffset(new[] { 2020, 1, 6, 23, 59, 59, 999, 9999 }),
                    0
                },
                {
                    BuildDateTimeOffset(new[] { 2020, 1, 6, 0, 0, 0, 0, 1 }),
                    BuildDateTimeOffset(new[] { 2020, 1, 6, 23, 59, 59, 999, 9999 }),
                    0
                },
                {
                    BuildDateTimeOffset(new[] { 2020, 1, 6, 12, 0, 0, 0, 0 }),
                    BuildDateTimeOffset(new[] { 2020, 1, 6, 23, 59, 59, 999, 9999 }),
                    0
                },
                {
                    BuildDateTimeOffset(new[] { 2020, 1, 6, 23, 59, 59, 999, 9998 }),
                    BuildDateTimeOffset(new[] { 2020, 1, 6, 23, 59, 59, 999, 9999 }),
                    0
                },
                {
                    BuildDateTimeOffset(new[] { 2020, 1, 6, 23, 59, 59, 999, 9999 }),
                    BuildDateTimeOffset(new[] { 2020, 1, 6, 23, 59, 59, 999, 9999 }),
                    0
                },
                {
                    BuildDateTimeOffset(new[] { 2020, 1, 7, 0, 0, 0, 0, 0 }),
                    BuildDateTimeOffset(new[] { 2020, 1, 6, 23, 59, 59, 999, 9999 }),
                    1
                },
                {
                    BuildDateTimeOffset(new[] { 2020, 1, 7, 0, 0, 0, 0, 1 }),
                    BuildDateTimeOffset(new[] { 2020, 1, 6, 23, 59, 59, 999, 9999 }),
                    1
                },
            };
        }

        public abstract class DateModificationExtensionMethodTestFixture<T>
        {
            public abstract DateTimeOffset InvokeMethod(DateTimeOffset value);

            public abstract TheoryData<DateTimeOffset, DateTimeOffset, int>  DataForTestingExtensionMethod { get; }

            [Theory]
            [InstanceMemberData(nameof(DataForTestingExtensionMethod))]
            public void ExtensionMethod(
                DateTimeOffset input,
                DateTimeOffset target,
                int comparison)
            {
                var transformedInput = InvokeMethod(input);

                Assert.Equal(comparison, transformedInput.CompareTo(target));
            }

            protected static DateTimeOffset BuildDateTimeOffset(int[] dateArguments)
            {
                var inputDate = new DateTimeOffset(
                    dateArguments[0],
                    dateArguments[1],
                    dateArguments[2],
                    dateArguments[3],
                    dateArguments[4],
                    dateArguments[5],
                    dateArguments[6],
                    TimeSpan.Zero
                ).AddTicks(dateArguments[7]);
                return inputDate;
            }

        }
    }


    public class DateTimeExtensionsTest
    {
        [Theory]
        [InlineData(
          nameof(DateTimeExtensions.ToEndOfDay),
          new[] { 2020, 1, 5, 23, 59, 59, 999, 9998 },
          new[] { 2020, 1, 6, 23, 59, 59, 999, 9999 },
          -1
        )]
        [InlineData(
          nameof(DateTimeExtensions.ToEndOfDay),
          new[] { 2020, 1, 5, 23, 59, 59, 999, 9999 },
          new[] { 2020, 1, 6, 23, 59, 59, 999, 9999 },
          -1
        )]
        [InlineData(
          nameof(DateTimeExtensions.ToEndOfDay),
          new[] { 2020, 1, 6, 0, 0, 0, 0, 0 },
          new[] { 2020, 1, 6, 23, 59, 59, 999, 9999 },
          0
        )]
        [InlineData(
          nameof(DateTimeExtensions.ToEndOfDay),
          new[] { 2020, 1, 6, 0, 0, 0, 0, 1 },
          new[] { 2020, 1, 6, 23, 59, 59, 999, 9999 },
          0
        )]
        [InlineData(
          nameof(DateTimeExtensions.ToEndOfDay),
          new[] { 2020, 1, 6, 12, 0, 0, 0, 0 },
          new[] { 2020, 1, 6, 23, 59, 59, 999, 9999 },
          0
        )]
        [InlineData(
          nameof(DateTimeExtensions.ToEndOfDay),
          new[] { 2020, 1, 6, 23, 59, 59, 999, 9998 },
          new[] { 2020, 1, 6, 23, 59, 59, 999, 9999 },
          0
        )]
        [InlineData(
          nameof(DateTimeExtensions.ToEndOfDay),
          new[] { 2020, 1, 6, 23, 59, 59, 999, 9999 },
          new[] { 2020, 1, 6, 23, 59, 59, 999, 9999 },
          0
        )]
        [InlineData(
          nameof(DateTimeExtensions.ToEndOfDay),
          new[] { 2020, 1, 7, 0, 0, 0, 0, 0 },
          new[] { 2020, 1, 6, 23, 59, 59, 999, 9999 },
          1
        )]
        [InlineData(
          nameof(DateTimeExtensions.ToEndOfDay),
          new[] { 2020, 1, 7, 0, 0, 0, 0, 1 },
          new[] { 2020, 1, 6, 23, 59, 59, 999, 9999 },
          1
        )]
        [InlineData(
          nameof(DateTimeExtensions.ToStartOfDay),
          new[] { 2020, 1, 5, 23, 59, 59, 999, 9998 },
          new[] { 2020, 1, 6, 00, 00, 00, 000, 0000 },
          -1
        )]
        [InlineData(
          nameof(DateTimeExtensions.ToStartOfDay),
          new[] { 2020, 1, 5, 23, 59, 59, 999, 9999 },
          new[] { 2020, 1, 6, 00, 00, 00, 000, 0000 },
          -1
        )]
        [InlineData(
          nameof(DateTimeExtensions.ToStartOfDay),
          new[] { 2020, 1, 6, 0, 0, 0, 0, 0 },
          new[] { 2020, 1, 6, 00, 00, 00, 000, 0000 },
          0
        )]
        [InlineData(
          nameof(DateTimeExtensions.ToStartOfDay),
          new[] { 2020, 1, 6, 0, 0, 0, 0, 1 },
          new[] { 2020, 1, 6, 00, 00, 00, 000, 0000 },
          0
        )]
        [InlineData(
          nameof(DateTimeExtensions.ToStartOfDay),
          new[] { 2020, 1, 6, 12, 0, 0, 0, 0 },
          new[] { 2020, 1, 6, 00, 00, 00, 000, 0000 },
          0
        )]
        [InlineData(
          nameof(DateTimeExtensions.ToStartOfDay),
          new[] { 2020, 1, 6, 23, 59, 59, 999, 9998 },
          new[] { 2020, 1, 6, 00, 00, 00, 000, 0000 },
          0
        )]
        [InlineData(
          nameof(DateTimeExtensions.ToStartOfDay),
          new[] { 2020, 1, 6, 23, 59, 59, 999, 9999 },
          new[] { 2020, 1, 6, 00, 00, 00, 000, 0000 },
          0
        )]
        [InlineData(
          nameof(DateTimeExtensions.ToStartOfDay),
          new[] { 2020, 1, 7, 0, 0, 0, 0, 0 },
          new[] { 2020, 1, 6, 00, 00, 00, 000, 0000 },
          1
        )]
        [InlineData(
          nameof(DateTimeExtensions.ToStartOfDay),
          new[] { 2020, 1, 7, 0, 0, 0, 0, 1 },
          new[] { 2020, 1, 6, 00, 00, 00, 000, 0000 },
          1
        )]
        [InlineData(
          nameof(DateTimeExtensions.ToStartOfMonth),
          new[] { 2019, 12, 05, 23, 59, 59, 999, 9998 },
          new[] { 2020, 01, 01, 00, 00, 00, 000, 0000 },
          -1
        )]
        [InlineData(
          nameof(DateTimeExtensions.ToStartOfMonth),
          new[] { 2020, 01, 05, 23, 59, 59, 999, 9998 },
          new[] { 2020, 01, 01, 00, 00, 00, 000, 0000 },
          0
        )]
        [InlineData(
          nameof(DateTimeExtensions.ToStartOfMonth),
          new[] { 2020, 01, 05, 23, 59, 59, 999, 9999 },
          new[] { 2020, 01, 01, 00, 00, 00, 000, 0000 },
          0
        )]
        [InlineData(
          nameof(DateTimeExtensions.ToStartOfMonth),
          new[] { 2020, 1, 6, 0, 0, 0, 0, 0 },
          new[] { 2020, 01, 01, 00, 00, 00, 000, 0000 },
          0
        )]
        [InlineData(
          nameof(DateTimeExtensions.ToStartOfMonth),
          new[] { 2020, 1, 6, 0, 0, 0, 0, 1 },
          new[] { 2020, 01, 01, 00, 00, 00, 000, 0000 },
          0
        )]
        [InlineData(
          nameof(DateTimeExtensions.ToStartOfMonth),
          new[] { 2020, 1, 6, 12, 0, 0, 0, 0 },
          new[] { 2020, 01, 01, 00, 00, 00, 000, 0000 },
          0
        )]
        [InlineData(
          nameof(DateTimeExtensions.ToStartOfMonth),
          new[] { 2020, 1, 6, 23, 59, 59, 999, 9998 },
          new[] { 2020, 01, 01, 00, 00, 00, 000, 0000 },
          0
        )]
        [InlineData(
          nameof(DateTimeExtensions.ToStartOfMonth),
          new[] { 2020, 1, 6, 23, 59, 59, 999, 9999 },
          new[] { 2020, 01, 01, 00, 00, 00, 000, 0000 },
          0
        )]
        [InlineData(
          nameof(DateTimeExtensions.ToStartOfMonth),
          new[] { 2020, 1, 7, 0, 0, 0, 0, 0 },
          new[] { 2020, 01, 01, 00, 00, 00, 000, 0000 },
          0
        )]
        [InlineData(
          nameof(DateTimeExtensions.ToStartOfMonth),
          new[] { 2020, 1, 7, 0, 0, 0, 0, 1 },
          new[] { 2020, 01, 01, 00, 00, 00, 000, 0000 },
          0
        )]
        [InlineData(
          nameof(DateTimeExtensions.ToStartOfMonth),
          new[] { 2020, 2, 7, 0, 0, 0, 0, 0 },
          new[] { 2020, 01, 01, 00, 00, 00, 000, 0000 },
          1
        )]
        [InlineData(
          nameof(DateTimeExtensions.ToStartOfMonth),
          new[] { 2020, 2, 7, 0, 0, 0, 0, 1 },
          new[] { 2020, 01, 01, 00, 00, 00, 000, 0000 },
          1
        )]
        public void EndOfDayValuesCompareCorrectly(
          string methodName,
          int[] dateArguments,
          int[] targetDateArguments,
          int comparison
        )
        {
            AssertDateExtensionMethod(
              dateArguments,
              targetDateArguments,
              comparison,
              date => InvokeExtensionMethod(date, methodName)
            );
        }

        private static DateTimeOffset InvokeExtensionMethod(
          DateTimeOffset date,
          string methodName
        )
        {
            return (DateTimeOffset)typeof(DateTimeExtensions).GetMethod(methodName).
              Invoke(date, new object[] { date });
        }

        private static DateTimeOffset BuildDateTimeOffset(int[] dateArguments)
        {
            var inputDate = new DateTimeOffset(
              dateArguments[0],
              dateArguments[1],
              dateArguments[2],
              dateArguments[3],
              dateArguments[4],
              dateArguments[5],
              dateArguments[6],
              TimeSpan.Zero
            ).AddTicks(dateArguments[7]);
            return inputDate;
        }

        private void AssertDateExtensionMethod(
          int[] dateArguments,
          int[] targetDateArguments,
          int comparison,
          Func<DateTimeOffset, DateTimeOffset> extensionMethod
        )
        {
            var inputDate = BuildDateTimeOffset(dateArguments);
            var targetDate = BuildDateTimeOffset(targetDateArguments);

            var transformedDate = extensionMethod(inputDate);

            Assert.Equal(comparison, transformedDate.CompareTo(targetDate));
        }
    }
}
