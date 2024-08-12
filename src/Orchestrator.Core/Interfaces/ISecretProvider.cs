namespace Orchestrator.Core.Interfaces;

public interface ISecretProvider
{
    Task<IDictionary<string, object>> GetSecret();
}