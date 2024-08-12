using MongoDB.Bson.Serialization.Attributes;
using SharedUtilLibrary;

namespace Orchestrator.Domain.Entities;

[BsonIgnoreExtraElements]
[BsonCollection("schedule")]
public class Schedule : Document
{
    public required string ScheduleId { get; set; } = null!;
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required string CronExpression { get; set; }
    public bool IsActive { get; set; }
    public string JobId { get; set; }
    public required string Topic { get; set; }
    public required string ProcessCode { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
    public int Priority { get; set; }
}
