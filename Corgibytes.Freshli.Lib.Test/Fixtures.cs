using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using HtmlAgilityPack;

using Polly;
using Polly.Registry;

using Xunit.Abstractions;

using Corgibytes.Freshli.Lib.Languages.Ruby;

namespace Corgibytes.Freshli.Lib.Test
{
    public class Fixtures
    {
        public static PolicyRegistry PolicyRegistry { get; private set; }

        [ModuleInitializer]
        public static void ModuleInitializer()
        {
            InitializePolicyRegistry();
        }

        public static void InitializePolicyRegistry()
        {
            var policy = Policy.BulkheadAsync<HtmlDocument>(5);
            PolicyRegistry = new PolicyRegistry();
            PolicyRegistry.Add("RubyGemsHttpRequests", policy);
        }

        public static IServiceProvider BuildServiceProviderWith(ITestOutputHelper outputHelper)
        {
            _ = PolicyRegistry ?? throw new NullReferenceException("PolicyRegistry must have a value");

            var services = new ServiceCollection();

            services.AddLogging(builder => builder.AddXUnit(outputHelper));
            services.AddSingleton<IReadOnlyPolicyRegistry<string>>(PolicyRegistry);
            services.AddTransient<RubyGemsRepository>();

            return services.BuildServiceProvider();
        }

        public static string Path(params string[] values)
        {
            var assemblyPath = System.Reflection.Assembly.
              GetExecutingAssembly().Location;

            var components = new List<string>() {
                Directory.GetParent(assemblyPath).ToString(), "fixtures"
            };
            components.AddRange(values);

            return System.IO.Path.Combine(components.ToArray());
        }
    }
}
