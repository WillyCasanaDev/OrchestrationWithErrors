namespace Orchestrator.Infrastructure.Options;

public class SendGridMulesoftOptions
{
    public string? ApiKey { get; set; }
    public string? BaseUrl { get; set; }
    public List<string>? DefaultRecipients { get; set; }
    public string? EmailFrom { get; set; }
    public List<EndpointOptions>? Endpoints { get; set; }
}