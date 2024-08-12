using Microsoft.Extensions.Logging;
using Orchestrator.Core.Interfaces;
using Orchestrator.CrossCutting.Exceptions;

namespace Orchestrator.Core.Services;

public class ExampleService(ILogger<ExampleService> logger) : IExampleService
{
    public async Task<string> CreateExampleAsync(string name, string description)
    {
        try
        {
            return "Example created successfully!";
        }
        catch (BusinessException e)
        {
            logger.LogError(e, "Error creating Example");
            throw;
        }
    }
}