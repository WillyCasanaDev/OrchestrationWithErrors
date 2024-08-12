using Microsoft.Extensions.DependencyInjection;
using Orchestrator.Core.Interfaces;
using Orchestrator.Core.Services;

namespace Orchestrator.Core;

public static class IoCExtensions
{
    public static IServiceCollection AddCoreDependencies(this IServiceCollection services)
    {
        services.AddScoped<IExampleService, ExampleService>();
        return services;
    }
}