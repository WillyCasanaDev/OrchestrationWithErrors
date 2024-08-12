using System.Net;

namespace Orchestrator.CrossCutting.Exceptions;

public class ApplicationException(string message, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
    : BaseException(message, statusCode);