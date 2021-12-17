using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using DiffEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using VerifyTests;

namespace Corgibytes.Freshli.Lib.Test
{
    public class Initializer
    {
        [ModuleInitializer]
        public static void Initialize()
        {
            DiffRunner.Disabled = true;
            VerifyDiffPlex.Initialize();

            VerifierSettings.DisableClipboard();
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
