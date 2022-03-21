using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Pipeliner.Models;

public class CustomTask
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = null!;
    public string UserId { get; set; } = null!; // created by
    public string Name { get; set; } = null!;
    public decimal AverageTime { get; set; }

    public CustomTask(string userId, string name, decimal averageTime)
    {
        UserId = userId;
        Name = name;
        AverageTime = averageTime;
    }
}
