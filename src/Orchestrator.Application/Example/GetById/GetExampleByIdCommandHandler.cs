using MediatR;
using ApplicationException = Orchestrator.CrossCutting.Exceptions.ApplicationException;

namespace Orchestrator.Application.Example.GetById;

public class GetExampleByIdCommandHandler : IRequestHandler<GetExampleByIdCommandRequest,string>
{
    public async Task<string> Handle(GetExampleByIdCommandRequest request, CancellationToken cancellationToken)
    {
         throw new ApplicationException("Error retrieving Example");
    }
}

public class GetExampleByIdCommandRequest : IRequest<string>
{
    public int Id { get; set; }
}