using Asp.Versioning;

namespace Orchestrator.ApiHost.Controllers;

public static class EndpointConfiguration
{
    public static void ConfigureEndpoints(WebApplication app)
    {
        var apiVersionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1))
            .HasApiVersion(new ApiVersion(2))
            .ReportApiVersions()
            .Build();
        
        var versionedGroup = app.MapGroup("api/v{apiVersion:apiVersion}").WithApiVersionSet(apiVersionSet);
        versionedGroup.ExampleRoutes();
        versionedGroup.ScheduleRoutes();
    }
}