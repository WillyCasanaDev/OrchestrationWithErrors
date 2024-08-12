namespace Orchestrator.Infrastructure.Options;

public class EventGridOptions
{
    public string? SubscriptionEndpoint { get; set; }
    public string? SubscriptionKey { get; set; }
    public string? TopicEndpoint { get; set; }
    public string? TopicKey { get; set; }
}