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
        public class ToEndOfDay : DateModificationExtensionMethodTestFixture<ToEndOfDay>
        {
            public override DateTimeOffset InvokeMethod(DateTimeOffset value) => value.ToEndOfDay();

            public override TheoryData<IList<int>, IList<int>, int> DataForTestingExtensionMethod => new()
            {
                {
                    new[] { 2020, 1, 5, 23, 59, 59, 999, 9998 },
                    new[] { 2020, 1, 6, 23, 59, 59, 999, 9999 },
                    -1
                },
                {
                    new[] { 2020, 1, 5, 23, 59, 59, 999, 9999 },
                    new[] { 2020, 1, 6, 23, 59, 59, 999, 9999 },
                    -1
                },
                {
                    new[] { 2020, 1, 6, 0, 0, 0, 0, 0 },
                    new[] { 2020, 1, 6, 23, 59, 59, 999, 9999 },
                    0
                },
                {
                    new[] { 2020, 1, 6, 0, 0, 0, 0, 1 },
                    new[] { 2020, 1, 6, 23, 59, 59, 999, 9999 },
                    0
                },
                {
                    new[] { 2020, 1, 6, 12, 0, 0, 0, 0 },
                    new[] { 2020, 1, 6, 23, 59, 59, 999, 9999 },
                    0
                },
                {
                    new[] { 2020, 1, 6, 23, 59, 59, 999, 9998 },
                    new[] { 2020, 1, 6, 23, 59, 59, 999, 9999 },
                    0
                },
                {
                    new[] { 2020, 1, 6, 23, 59, 59, 999, 9999 },
                    new[] { 2020, 1, 6, 23, 59, 59, 999, 9999 },
                    0
                },
                {
                    new[] { 2020, 1, 7, 0, 0, 0, 0, 0 },
                    new[] { 2020, 1, 6, 23, 59, 59, 999, 9999 },
                    1
                },
                {
                    new[] { 2020, 1, 7, 0, 0, 0, 0, 1 },
                    new[] { 2020, 1, 6, 23, 59, 59, 999, 9999 },
                    1
                },
            };
        }

        public class ToStartOfDay : DateModificationExtensionMethodTestFixture<ToStartOfDay>
        {
            public override DateTimeOffset InvokeMethod(DateTimeOffset value) => value.ToStartOfDay();

            public override TheoryData<IList<int>, IList<int>, int> DataForTestingExtensionMethod => new()
            {
                {
                    new[] { 2020, 1, 5, 23, 59, 59, 999, 9998 },
                    new[] { 2020, 1, 6, 00, 00, 00, 000, 0000 },
                    -1
                },
                {
                    new[] { 2020, 1, 5, 23, 59, 59, 999, 9999 },
                    new[] { 2020, 1, 6, 00, 00, 00, 000, 0000 },
                    -1
                },
                {
                    new[] { 2020, 1, 6, 0, 0, 0, 0, 0 },
                    new[] { 2020, 1, 6, 00, 00, 00, 000, 0000 },
                    0
                },
                {
                    new[] { 2020, 1, 6, 0, 0, 0, 0, 1 },
                    new[] { 2020, 1, 6, 00, 00, 00, 000, 0000 },
                    0
                },
                {
                    new[] { 2020, 1, 6, 12, 0, 0, 0, 0 },
                    new[] { 2020, 1, 6, 00, 00, 00, 000, 0000 },
                    0
                },
                {
                    new[] { 2020, 1, 6, 23, 59, 59, 999, 9998 },
                    new[] { 2020, 1, 6, 00, 00, 00, 000, 0000 },
                    0
                },
                {
                    new[] { 2020, 1, 6, 23, 59, 59, 999, 9999 },
                    new[] { 2020, 1, 6, 00, 00, 00, 000, 0000 },
                    0
                },
                {
                    new[] { 2020, 1, 7, 0, 0, 0, 0, 0 },
                    new[] { 2020, 1, 6, 00, 00, 00, 000, 0000 },
                    1
                },
                {
                    new[] { 2020, 1, 7, 0, 0, 0, 0, 1 },
                    new[] { 2020, 1, 6, 00, 00, 00, 000, 0000 },
                    1
                }
            };
        }

        public class ToStartOfMonth : DateModificationExtensionMethodTestFixture<ToStartOfMonth>
        {
            public override DateTimeOffset InvokeMethod(DateTimeOffset value) => value.ToStartOfMonth();

            public override TheoryData<IList<int>, IList<int>, int> DataForTestingExtensionMethod => new()
            {
                {
                    new[] { 2019, 12, 05, 23, 59, 59, 999, 9998 },
                    new[] { 2020, 01, 01, 00, 00, 00, 000, 0000 },
                    -1
                },
                {
                    new[] { 2020, 01, 05, 23, 59, 59, 999, 9998 },
                    new[] { 2020, 01, 01, 00, 00, 00, 000, 0000 },
                    0
                },
                {
                    new[] { 2020, 01, 05, 23, 59, 59, 999, 9999 },
                    new[] { 2020, 01, 01, 00, 00, 00, 000, 0000 },
                    0
                },
                {
                    new[] { 2020, 1, 6, 0, 0, 0, 0, 0 },
                    new[] { 2020, 01, 01, 00, 00, 00, 000, 0000 },
                    0
                },
                {
                    new[] { 2020, 1, 6, 0, 0, 0, 0, 1 },
                    new[] { 2020, 01, 01, 00, 00, 00, 000, 0000 },
                    0
                },
                {
                    new[] { 2020, 1, 6, 12, 0, 0, 0, 0 },
                    new[] { 2020, 01, 01, 00, 00, 00, 000, 0000 },
                    0
                },
                {
                    new[] { 2020, 1, 6, 23, 59, 59, 999, 9998 },
                    new[] { 2020, 01, 01, 00, 00, 00, 000, 0000 },
                    0
                },
                {
                    new[] { 2020, 1, 6, 23, 59, 59, 999, 9999 },
                    new[] { 2020, 01, 01, 00, 00, 00, 000, 0000 },
                    0
                },
                {
                    new[] { 2020, 1, 7, 0, 0, 0, 0, 0 },
                    new[] { 2020, 01, 01, 00, 00, 00, 000, 0000 },
                    0
                },
                {
                    new[] { 2020, 1, 7, 0, 0, 0, 0, 1 },
                    new[] { 2020, 01, 01, 00, 00, 00, 000, 0000 },
                    0
                },
                {
                    new[] { 2020, 2, 7, 0, 0, 0, 0, 0 },
                    new[] { 2020, 01, 01, 00, 00, 00, 000, 0000 },
                    1
                },
                {
                    new[] { 2020, 2, 7, 0, 0, 0, 0, 1 },
                    new[] { 2020, 01, 01, 00, 00, 00, 000, 0000 },
                    1
                }
            };
        }

        public abstract class DateModificationExtensionMethodTestFixture<T>
        {
            public abstract DateTimeOffset InvokeMethod(DateTimeOffset value);

            public abstract TheoryData<IList<int>, IList<int>, int> DataForTestingExtensionMethod { get; }

            [Theory]
            [InstanceMemberData(nameof(DataForTestingExtensionMethod))]
            public void ExtensionMethod(
                int[] inputDateComponents,
                int[] targetDateComponents,
                int comparison)
            {
                var inputDate = BuildDateTimeOffset(inputDateComponents);
                var targetDate = BuildDateTimeOffset(targetDateComponents);
                var transformedInputDate = InvokeMethod(inputDate);

                Assert.Equal(comparison, transformedInputDate.CompareTo(targetDate));
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
}
