using System.Net;

namespace Orchestrator.CrossCutting.Exceptions;

public class InfrastructureException(string message, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
    : BaseException(message, statusCode);