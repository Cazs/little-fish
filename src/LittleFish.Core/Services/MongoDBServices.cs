using LittleFish.Core.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;

namespace LittleFish.Core.Services;

public class MongoDBService {
    private readonly IMongoCollection<User> _userCollection;

    public MongoDBService(IOptions<MongoDBSettings> mongoDBSettings) {
        MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
        IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
        _userCollection = database.GetCollection<User>(mongoDBSettings.Value.CollectionName);
    }

    public async Task<List<User>> GetAsync() {
        return await _userCollection.Find(new BsonDocument()).ToListAsync();
    }

    public async Task CreateAsync(User user) {
        await _userCollection.InsertOneAsync(user);
        return;
    }
    
    public async Task UpdateAsync(string id, User user) {
        FilterDefinition<User> filter = Builders<User>.Filter.Eq("Id", id);
        var updateDefinition = new List<UpdateDefinition<User>>();
        
        foreach (var dataField in user.ToBsonDocument()) {
            if(dataField.Value != null && !(dataField.Value is MongoDB.Bson.BsonNull)) {
                updateDefinition.Add(Builders<User>.Update.Set(dataField.Name, dataField.Value));
            }
        }

        var combinedUpdate = Builders<User>.Update.Combine(updateDefinition);
        await _userCollection.UpdateManyAsync(filter, combinedUpdate);
        return;
    }

    public async Task DeleteAsync(string id) {
        FilterDefinition<User> filter = Builders<User>.Filter.Eq("Id", id);
        await _userCollection.DeleteOneAsync(filter);
        return;
    }
}