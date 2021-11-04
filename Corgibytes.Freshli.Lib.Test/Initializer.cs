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
        }
    }
}
