namespace Orchestrator.Infrastructure.Options;

public class AzureAdB2COptions
{
    public string? ClientId { get; set; }
    public string? ClientName { get; set; }
    public string? ClientSecret { get; set; }
    public string? Instance { get; set; }
    public string? Scope { get; set; }
    public string? TenantId { get; set; }
}
