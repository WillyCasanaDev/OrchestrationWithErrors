namespace Orchestrator.Infrastructure;

public class HangfireConfigurationOptions
{
    public int ToleranceDelayExecutionInMinutes { get; set; } = 5; 
    public int InvisibilityTimeoutInMinutes { get; set; } = 5;
    public List<int>? TokenExpirationNotificationDays { get; set; }

    public string[]? Queues { get; set; }
    public Dashboard Dashboard { get; set; } = new();
}

public class Dashboard
{
    public string User { get; set; } = "admin";
    public string Password { get; set; } = "admin";
    public bool IsReadOnly { get; set; }
}
