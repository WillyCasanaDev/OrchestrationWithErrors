using System.Net;

namespace Orchestrator.CrossCutting.Exceptions;

public class BaseException(string message, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
    : Exception(message)
{
    public HttpStatusCode StatusCode { get; } = statusCode;
}