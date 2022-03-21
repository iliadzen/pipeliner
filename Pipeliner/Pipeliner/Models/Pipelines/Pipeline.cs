using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Pipeliner.Models;

public class Pipeline
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = null!;
    public string UserId { get; set; } = null!; // created by

    public Pipeline(string userId)
    {
        UserId = userId;
    }
}
