using MediatR;
using Orchestrator.Core.Interfaces;

namespace Orchestrator.Application.Example.Create;

public class CreateExampleCommandHandler(IExampleService exampleService)
    : IRequestHandler<CreateExampleCommandRequest, string>
{
    public async Task<string> Handle(CreateExampleCommandRequest request, CancellationToken cancellationToken)
    {
        return await exampleService.CreateExampleAsync(request.Name!, request.Description!);
    }
}

public class CreateExampleCommandRequest : IRequest<string>
{
    public string? Name { get; set; }
    public string? Description { get; set; }
}