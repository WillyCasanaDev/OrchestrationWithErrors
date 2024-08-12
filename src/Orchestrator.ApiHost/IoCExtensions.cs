using System.Net;
using Orchestrator.ApiHost.Middleware;
using Hellang.Middleware.ProblemDetails;
using Orchestrator.CrossCutting.Exceptions;
using ApplicationException = Orchestrator.CrossCutting.Exceptions.ApplicationException;

namespace Orchestrator.ApiHost;

public static class IoCExtensions
{
    public static void UseErrorHandlerMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<ErrorHandler>();
    }
    
    public static void AddProblemDetailsMiddleware(this IServiceCollection service)
    {
        service.AddProblemDetails(setup =>
        {
            
            setup.IncludeExceptionDetails = (ctx, ex) => true;
            
            setup.MapToStatusCode<InfrastructureException>((int)HttpStatusCode.InternalServerError);
            setup.MapToStatusCode<BusinessException>((int)HttpStatusCode.BadRequest);
            setup.MapToStatusCode<ApplicationException>((int)HttpStatusCode.BadRequest);
            setup.MapToStatusCode<Exception>((int)HttpStatusCode.InternalServerError);
            
            setup.OnBeforeWriteDetails = (ctx, problem) =>
            {
                if (problem.Extensions.ContainsKey("exceptionDetails"))
                {
                    problem.Extensions.Remove("exceptionDetails");
                }
            };
        });
    }
}