using Pipeliner.Models;

namespace Pipeliner.Services;

public interface IPipelineService
{
    Task<List<Pipeline>> Get();
    Task<Pipeline?> Get(string id);
    Task Create(Pipeline pipeline);
    Task<RunRecord> Run(Pipeline pipeline);
}
