namespace Orchestrator.Infrastructure.Options;

public class VaultOptions
{
    public string? Profile { get; set; }
    public string? Token { get; set; }
    public string? Uri { get; set; } 
    public string? MountPoint { get; set; }
    public string? ApplicationName { get; set; } 
}