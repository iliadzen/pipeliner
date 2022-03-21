using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Pipeliner.Models;

namespace Pipeliner.Services;

public class UsersService
{
    private readonly IMongoCollection<User> _usersCollection;

    public UsersService(IOptions<DatabaseSettings> databaseSettings)
    {
        var mongoClient = new MongoClient(
            databaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            databaseSettings.Value.DatabaseName);

        _usersCollection = mongoDatabase.GetCollection<User>("Users");
    }

    public async Task<List<User>> Get() =>
        await _usersCollection.Find(_ => true).ToListAsync();

    public async Task<User?> Get(string id) =>
        await _usersCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
}
