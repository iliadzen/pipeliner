using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Pipeliner.Models;

namespace Pipeliner.Services;

public class TaskPipelineMappingsService
{
    private readonly IMongoCollection<TaskPipelineMapping> _mappingsCollection;

    public TaskPipelineMappingsService(
        IOptions<DatabaseSettings> databaseSettings)
    {
        var mongoClient = new MongoClient(
            databaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            databaseSettings.Value.DatabaseName);

        _mappingsCollection = mongoDatabase.GetCollection<TaskPipelineMapping>("TaskPipelineMapping");
    }

    public async Task<List<TaskPipelineMapping>> Get() =>
        await _mappingsCollection.Find(_ => true).ToListAsync();

    public async Task<List<TaskPipelineMapping>> Get(string pipelineId) =>
        await _mappingsCollection.Find(x => x.PipelineId == pipelineId).ToListAsync();

    public async Task<TaskPipelineMapping?> Get(string taskId, string pipelineId) =>
        await _mappingsCollection.Find(x => x.PipelineId == pipelineId && x.TaskId == taskId).FirstOrDefaultAsync();

    public async Task Create(TaskPipelineMapping mapping) =>
        await _mappingsCollection.InsertOneAsync(mapping);

    public async Task Delete(string id) =>
        await _mappingsCollection.DeleteOneAsync(x => x.Id == id);
}
