namespace Orchestrator.ApiHost.Middleware;

public class ErrorHandler(RequestDelegate next, ILogger<ErrorHandler> logger)
{
    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await next(httpContext);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "[HttpRequestError] {CustomExceptionName} with message: {Message} \nStack Trace: {StackTrace}", ex.GetType().Name,
                ex.Message, ex.StackTrace);
            throw;
        }
    }
}