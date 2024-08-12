namespace Orchestrator.Core.Interfaces;

public interface IExampleService
{
    Task<string> CreateExampleAsync(string name, string description);
}