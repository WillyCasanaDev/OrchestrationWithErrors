namespace Orchestrator.Infrastructure.Options;

public class RetryPolicyOptions
{
    public int RetryCount { get; set; }
    public int SleepDurationProvider { get; set; }
}