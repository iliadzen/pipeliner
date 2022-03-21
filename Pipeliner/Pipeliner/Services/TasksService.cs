using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Pipeliner.Models;

namespace Pipeliner.Services;

public class TasksService
{
    private readonly IMongoCollection<CustomTask> _tasksCollection;

    public TasksService(IOptions<DatabaseSettings> databaseSettings)
    {
        var mongoClient = new MongoClient(
            databaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            databaseSettings.Value.DatabaseName);

        _tasksCollection = mongoDatabase.GetCollection<CustomTask>("Tasks");
    }

    public async Task<List<CustomTask>> Get() =>
        await _tasksCollection.Find(_ => true).ToListAsync();

    public async Task<CustomTask?> Get(string id) =>
        await _tasksCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task Create(CustomTask task)
    {
        await _tasksCollection.InsertOneAsync(task);
    }

    public async Task Update(string id, CustomTask task) =>
        await _tasksCollection.ReplaceOneAsync(x => x.Id == id, task);
}
