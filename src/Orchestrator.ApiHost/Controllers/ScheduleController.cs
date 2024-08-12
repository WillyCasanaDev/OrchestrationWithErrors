using MediatR;
using Microsoft.AspNetCore.Mvc;
using Orchestrator.Application.Schedule.Create;

namespace Orchestrator.ApiHost;

public static class ScheduleController
{
    public static void ScheduleRoutes(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/schedules")
                       .WithTags("Schedules")
                       .WithOpenApi();

        group.MapPost("/create", async (CreateScheduleCommandRequest request, [FromServices] IMediator mediator) =>
            await mediator.Send(request)
        ).MapToApiVersion(1);
    }
}
