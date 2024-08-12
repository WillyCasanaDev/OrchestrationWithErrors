using MongoDB.Bson.Serialization.Attributes;
using SharedUtilLibrary;

namespace Orchestrator.Domain.Entities;

[BsonIgnoreExtraElements]
[BsonCollection("executedevent")]
public class ExecutedEvent : Document
{
    public required string ScheduleId { get; set; }
    public DateTime ExecutionTime { get; set; }
    public required string Status { get; set; }
    public int ReprocessCount { get; set; }
    public DateTime LastExecuteDate { get; set; }
    public string? ReprocessDetails { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
}