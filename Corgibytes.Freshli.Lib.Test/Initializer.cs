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

            VerifierSettings.ModifySerialization(settings =>
            {
                settings.DontScrubNumericIds();
                settings.DontIgnoreEmptyCollections();
                settings.DontScrubDateTimes();
                settings.DontIgnoreFalse();
                settings.MemberConverter<ScanResult, string>(
                    r => r.Filename,
                    (target, value) => value.Replace("\\", "/")
                );
            });

            VerifyMicrosoftLogging.Enable();
        }
    }
}
