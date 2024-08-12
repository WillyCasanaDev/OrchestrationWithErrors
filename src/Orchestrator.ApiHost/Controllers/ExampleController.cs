using MediatR;
using Microsoft.AspNetCore.Mvc;
using Orchestrator.Application.Example.Create;
using Orchestrator.Application.Example.GetById;
using ApplicationException = Orchestrator.CrossCutting.Exceptions.ApplicationException;

namespace Orchestrator.ApiHost.Controllers;

public static class ExampleController
{
    public static void ExampleRoutes(this IEndpointRouteBuilder app)
    {
        app.MapGet("/GetById",
            async (int id, [FromServices] IMediator mediator) =>
            await mediator.Send(new GetExampleByIdCommandRequest { Id = id })
        ).MapToApiVersion(1);
        
        app.MapGet("/GetById",
            async (int id, [FromServices] IMediator mediator) =>
            test()
        ).MapToApiVersion(2);

        app.MapPost("/Create", async (CreateExampleCommandRequest request, [FromServices] IMediator mediator) =>
            await mediator.Send(request)
        ).MapToApiVersion(1);
    }

    private static void test()
    {
        throw new ApplicationException("Test de error dentro del controlador");
    }
}