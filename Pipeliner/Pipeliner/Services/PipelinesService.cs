using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Pipeliner.Models;

namespace Pipeliner.Services;

public class PipelinesService : IPipelineService
{
    private readonly IMongoCollection<Pipeline> _pipelinesCollection;
    private readonly IMongoCollection<RunRecord> _pipelineRecordsCollection;

    public PipelinesService(
        IOptions<DatabaseSettings> databaseSettings)
    {
        var mongoClient = new MongoClient(
            databaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            databaseSettings.Value.DatabaseName);

        _pipelinesCollection = mongoDatabase.GetCollection<Pipeline>("Pipelines");
        _pipelineRecordsCollection = mongoDatabase.GetCollection<RunRecord>("PipelineRunTimes");
    }

    public async Task<List<Pipeline>> Get() =>
        await _pipelinesCollection.Find(_ => true).ToListAsync();

    public async Task<Pipeline?> Get(string id) =>
        await _pipelinesCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task Create(Pipeline pipeline) => 
        await _pipelinesCollection.InsertOneAsync(pipeline);

    public async Task<RunRecord> Run(Pipeline pipeline)
    {
        var sleepTime = new Random().Next(0, 10000);
        Thread.Sleep(sleepTime);
        var runTime = sleepTime/1000M;

        var record = new RunRecord(pipeline.Id, runTime);
        await _pipelineRecordsCollection.InsertOneAsync(record);
        return record;
    }
}
