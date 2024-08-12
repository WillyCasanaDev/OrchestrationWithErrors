using MongoDB.Bson.Serialization.Attributes;
using SharedUtilLibrary;

namespace Orchestrator.Domain.Entities;

[BsonIgnoreExtraElements]
[BsonCollection("executedevent")]
public class MessageQueue : Document
{
    public required string ProcessCode { get; set; }
    public DateTime MessageDate { get; set; }
    public string? Payload { get; set; }
    public required string Status { get; set; }
    public string? ErrorDetails { get; set; }
    public DateTime CreatedDate { get; set; }
}