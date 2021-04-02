using System;
using Corgibytes.Freshli.Lib.Util;
using Xunit;

namespace Corgibytes.Freshli.Lib.Test.Unit.Util {
  public class DateTimeExtensionsTest {
    public static TheoryData<int[], int[], int> EndOfDayTestData =>
      BuildTestData(
        TestDataDateArguments,
        EndOfDayDateArguments,
        EndOfDayTestDataResults
      );

    [Theory]
    [MemberData(nameof(EndOfDayTestData))]
    public void EndOfDayValuesCompareCorrectly(
      int[] dateArguments,
      int[] targetDateArguments,
      int comparison
    ) {
      AssertDateExtensionMethod(
        dateArguments,
        targetDateArguments,
        comparison,
        date => date.ToEndOfDay()
      );
    }

    public static TheoryData<int[], int[], int> StartOfDayTestData =>
      BuildTestData(
        TestDataDateArguments,
        BeginningOfDayDateArguments,
        StartOfDayTestDataResults
      );

    [Theory]
    [MemberData(nameof(StartOfDayTestData))]
    public void StartOfDayValuesCompareCorrectly(
      int[] dateArguments,
      int[] targetDateArguments,
      int comparison
    ) {
      AssertDateExtensionMethod(
        dateArguments,
        targetDateArguments,
        comparison,
        date => date.ToStartOfDay()
      );
    }

    public static TheoryData<int[], int[], int> StartOfMonthTestData =>
      BuildTestData(
        TestDataDateArguments,
        BeginningOfDayDateArguments,
        StartOfMonthTestDataResults
      );

    [Theory]
    [MemberData(nameof(StartOfMonthTestData))]
    public void StartOfMonthValuesCompareCorrectly(
      int[] dateArguments,
      int[] targetDateArguments,
      int comparison
    ) {
      AssertDateExtensionMethod(
        dateArguments,
        targetDateArguments,
        comparison,
        date => date.ToStartOfMonth()
      );
    }

    private static TheoryData<int[], int[], int> BuildTestData(
      int[][] testDataDateArguments,
      int[] targetDateArguments,
      int[] testDataExpectedResults
    ) {
      var result = new TheoryData<int[], int[], int>();

      for (var index = 0; index < testDataDateArguments.Length; index++) {
        result.Add(
          testDataDateArguments[index],
          targetDateArguments,
          testDataExpectedResults[index]
        );
      }

      return result;

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

    private static void AssertDateExtensionMethod(
      int[] dateArguments,
      int[] targetDateArguments,
      int comparison,
      Func<DateTimeOffset, DateTimeOffset> extensionMethod
    ) {
      var inputDate = BuildDateTimeOffset(dateArguments);
      var targetDate = BuildDateTimeOffset(targetDateArguments);

      var transformedDate = extensionMethod(inputDate);

      Assert.Equal(comparison, transformedDate.CompareTo(targetDate));
    }

    private static int[][] TestDataDateArguments {
      get {
        return new[] {
          new[] {2019, 12, 05, 23, 59, 59, 999, 9998},
          new[] {2020, 01, 05, 23, 59, 59, 999, 9998},
          new[] {2020, 01, 05, 23, 59, 59, 999, 9999},
          new[] {2020, 1, 6, 0, 0, 0, 0, 0},
          new[] {2020, 1, 6, 0, 0, 0, 0, 1},
          new[] {2020, 1, 6, 12, 0, 0, 0, 0},
          new[] {2020, 1, 6, 23, 59, 59, 999, 9998},
          new[] {2020, 1, 6, 23, 59, 59, 999, 9999},
          new[] {2020, 1, 7, 0, 0, 0, 0, 0},
          new[] {2020, 1, 7, 0, 0, 0, 0, 1},
          new[] {2020, 2, 7, 0, 0, 0, 0, 0},
          new[] {2020, 2, 7, 0, 0, 0, 0, 1},
        };
      }
    }

    private static int[] BeginningOfDayDateArguments {
      get {
        return new[] {2020, 1, 6, 00, 00, 00, 000, 0000};
      }
    }

    private static int[] EndOfDayDateArguments {
      get {
        return new[] {2020, 1, 6, 23, 59, 59, 999, 9999};
      }
    }

    private static int[] EndOfDayTestDataResults {
      get { return new[] {-1, -1, -1, 0, 0, 0, 0, 0, 1, 1, 1, 1}; }
    }

    private static int[] StartOfDayTestDataResults {
      get {
        return new[] {-1, -1, -1, 0, 0, 0, 0, 0, 1, 1};
      }
    }

    private static int[] StartOfMonthTestDataResults {
      get {
        return new[] {-1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1,};
      }
    }
  }
}
