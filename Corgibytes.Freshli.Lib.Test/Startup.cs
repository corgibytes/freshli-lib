using Microsoft.Extensions.DependencyInjection;

using Polly;
using Polly.Registry;

namespace Corgibytes.Freshli.Lib.Test
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            var policy = Policy.BulkheadAsync(5);
            var registry = new PolicyRegistry();
            registry.Add("RubyGemsHttpRequests", policy);

            services.AddSingleton<IReadOnlyPolicyRegistry<string>>(registry);
        }
    }
}
