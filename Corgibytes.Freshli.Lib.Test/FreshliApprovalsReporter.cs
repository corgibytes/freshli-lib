using ApprovalTests.Reporters;
using ApprovalTests.Reporters.TestFrameworks;

namespace Freshli.Test
{
    class FreshliApprovalsReporter : FirstWorkingReporter
    {
        public FreshliApprovalsReporter() : base(
          RiderReporter.INSTANCE,
          BeyondCompareReporter.INSTANCE,
          VisualStudioReporter.INSTANCE,
          XUnit2Reporter.INSTANCE,
          DiffReporter.INSTANCE
        )
        { }
    }
}
