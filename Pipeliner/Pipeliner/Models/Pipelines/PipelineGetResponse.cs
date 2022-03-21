namespace Pipeliner.Models;

public class PipelineGetResponse
{
    public string Id { get; set; }
    public List<string> TasksIds { get; set; }
    public decimal CountTime { get; set; }

    public PipelineGetResponse(Pipeline pipeline, List<string> tasksIds, decimal countTime)
    {
        Id = pipeline.Id;
        TasksIds = tasksIds;
        CountTime = countTime;
    }
}
