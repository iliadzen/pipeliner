namespace Pipeliner.Models;

public class PipelineRunResponse
{
    public decimal RunTime { get; set; }
    public PipelineRunResponse(decimal runTime) =>
        RunTime = runTime;
}
