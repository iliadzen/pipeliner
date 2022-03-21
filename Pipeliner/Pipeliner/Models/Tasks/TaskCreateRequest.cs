namespace Pipeliner.Models;

public class TaskCreateRequest
{
    public string UserId { get; set; }
    public string Name { get; set; }
    public decimal AverageTime { get; set; }
    public TaskCreateRequest(string userId, string name, decimal averageTime)
    {
        UserId = userId;
        Name = name;
        AverageTime = averageTime;
    }
}
