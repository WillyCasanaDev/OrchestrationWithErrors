namespace Orchestrator.Infrastructure.Options;

public class EnvironmentSetupOptions
{
    public int DefaultHttpTimeOutSeconds { get; set; }
    public int MaxDocumentSizeMb { get; set; }
    public string? Url { get; set; }
    public int WaitingToleranceInSeconds { get; set; }
}
