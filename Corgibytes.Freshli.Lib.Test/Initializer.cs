using System.Runtime.CompilerServices;
using VerifyTests;

namespace Corgibytes.Freshli.Lib.Test
{
    public class Initializer
    {
        [ModuleInitializer]
        public static void Initialize()
        {
            VerifyDiffPlex.Initialize();

            VerifierSettings.DontIgnoreEmptyCollections();
            VerifierSettings.DontScrubDateTimes();
            VerifierSettings.DontIgnoreFalse();
            VerifierSettings.MemberConverter<ScanResult, string>(
                r => r.Filename,
                (target, value) => value.Replace("\\", "/")
            );
        }
    }
}
