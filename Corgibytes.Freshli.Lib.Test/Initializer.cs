using System.Runtime.CompilerServices;
using VerifyTests;

namespace Corgibytes.Freshli.Lib.Test
{
    public class InitializeVerify
    {
        [ModuleInitializer]
        public static void Initialize()
        {
            VerifyDiffPlex.Initialize();
        }
    }
}
