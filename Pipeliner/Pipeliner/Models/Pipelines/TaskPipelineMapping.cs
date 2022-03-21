using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Pipeliner.Models;

public class TaskPipelineMapping
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = null!;
    public string TaskId { get; set; } = null!;
    public string PipelineId { get; set; } = null!;
    public TaskPipelineMapping(string taskId, string pipelineId)
    {
        TaskId = taskId;
        PipelineId = pipelineId;
    }
}
