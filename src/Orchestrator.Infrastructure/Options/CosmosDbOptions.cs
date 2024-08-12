namespace Orchestrator.Infrastructure.Options;

public class CosmosDbOptions
{
    public required string ConnectionString { get; set; } 
    public required string DatabaseName { get; set; }
}
