using Asp.Versioning;
using HealthChecks.UI.Client;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Orchestrator.ApiHost;
using Orchestrator.ApiHost.Controllers;
using Orchestrator.ApiHost.OpenApi;
using Orchestrator.Application;
using Orchestrator.Core;
using Orchestrator.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddLogging();

builder.Services.AddProblemDetailsMiddleware();

builder.Configuration.AddEnvironmentVariables();

builder.Services.AddInfrastructureDependencies(builder.Configuration);
builder.Services.AddCoreDependencies();
builder.Services.AddApplicationDependencies();


// Add Versioning
builder.Services.AddApiVersioning(opt =>
    {
        opt.DefaultApiVersion = new ApiVersion(1);
        opt.ApiVersionReader = new UrlSegmentApiVersionReader();
    }).AddMvc()
    .AddApiExplorer(opt =>
    {
        opt.GroupNameFormat = "'v'V";
        opt.SubstituteApiVersionInUrl = true;
    });
builder.Services.ConfigureOptions<ConfigureSwaggerGenOptions>();

var app = builder.Build();

app.UseHealthChecks("/health", new HealthCheckOptions
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});


app.UseProblemDetails();
app.UseHangfireDashboard(builder.Services);
app.UseErrorHandlerMiddleware();

app.UseHttpsRedirection();

EndpointConfiguration.ConfigureEndpoints(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(opt =>
    {
        var descriptions = app.DescribeApiVersions();

        foreach (var apiVersionDescription in descriptions)
        {
            var url = $"/swagger/{apiVersionDescription.GroupName}/swagger.json";
            var name = apiVersionDescription.GroupName.ToUpperInvariant();

            opt.SwaggerEndpoint(url, name);
        }
    });
}

app.Run();