using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Orchestrator.Infrastructure.Extensions;

public static class VersionHealthCheckExtension
{
    public static IServiceCollection AddVersionHealthCheck(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddHealthChecks()
            .AddCheck("Version", () =>
            {
                var version = Assembly.GetExecutingAssembly().GetName().Version?.ToString();
                return HealthCheckResult.Healthy($"Version: {version}");
            });

        return serviceCollection;
    }
}