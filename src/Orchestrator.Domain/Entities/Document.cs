using System.Text.Json.Serialization;
using MongoDB.Bson;
using SharedUtilLibrary;

namespace Orchestrator.Domain;

public class Document : IDocument
    {
        [JsonIgnore]
        public ObjectId Id { get; set; }
        [JsonIgnore]
        public DateTime CreatedAt => Id.CreationTime;
    }
