using ApprovalTests.Reporters;
using Freshli.Test;

[assembly: UseReporter(typeof(FreshliApprovalsReporter))]
[assembly: IgnoreLineEndingsAttribute(false)]
