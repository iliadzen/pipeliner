namespace Pipeliner.Models;

public class PipelineCreateRequest
{
    public string UserId { get; set; }
    public List<string> TasksIds { get; set; }

    public PipelineCreateRequest(string userId, List<string> tasksIds)
    {
        UserId = userId;
        TasksIds = tasksIds;
    }
}
