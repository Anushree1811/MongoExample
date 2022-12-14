using MongoExample.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace MongoExample.Services;

public class MongoDBService {

    private readonly IMongoCollection<UserTest> _usertestCollection;

    public MongoDBService(IOptions<MongoDBSettings> MongoDBSettings) {

        MongoClient client = new MongoClient(MongoDBSettings.Value.ConnectionURI);
        IMongoDatabase database = client.GetDatabase(MongoDBSettings.Value.DatabaseName);
        _usertestCollection = database.GetCollection<UserTest>(MongoDBSettings.Value.CollectionName);

    }

    public async Task CreateAsync(UserTest usertest) {
        await _usertestCollection.InsertOneAsync(usertest);
        return;
    }

    public async Task<List<UserTest>> GetAsync() {

        return await _usertestCollection.Find(new BsonDocument()).ToListAsync();
    }

    public async Task AddToUserTestAsync(string id, string uid) {

        FilterDefinition<UserTest> filter = Builders<UserTest>.Filter.Eq("Id", id);
        UpdateDefinition<UserTest> update = Builders<UserTest>.Update.AddToSet<string>("uid", uid);
        await _usertestCollection.UpdateOneAsync(filter, update);
        return;
            }


    public async Task DeleteAsync(string id) {

        FilterDefinition<UserTest> filter = Builders<UserTest>.Filter.Eq("Id", id);
        await _usertestCollection.DeleteOneAsync(filter);
        return;

    }
}