namespace Pipeliner.Models;

public class TaskGetResponse
{
    public string Id { get; set; }
    public string UserId { get; set; } // created by
    public string Name { get; set; }
    public decimal AverageTime { get; set; }

    public TaskGetResponse(CustomTask task)
    {
        Id = task.Id;
        UserId = task.UserId;
        Name = task.Name;
        AverageTime = task.AverageTime;
    }
}
