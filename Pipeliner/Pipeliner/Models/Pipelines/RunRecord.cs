using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Pipeliner.Models;

public class RunRecord
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = null!;
    public string EntityId { get; set; } = null!;
    public decimal RunTime { get; set; }

    public RunRecord(string entityId, decimal runTime)
    {
        EntityId = entityId;
        RunTime = runTime;
    }
}
