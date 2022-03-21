namespace Pipeliner.Models;

public class PipelineEditRequest
{
    public string UserId { get; set; }

    public PipelineEditRequest(string userId)
    {
        UserId = userId;
    }
}
