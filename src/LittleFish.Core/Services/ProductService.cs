using LittleFish.Core.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;

namespace LittleFish.Core.Services;

public class ProductsMongoDBService {
    private readonly IMongoCollection<Product> _productCollection;

    public ProductsMongoDBService(IOptions<MongoDBSettings> mongoDBSettings) {
        MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
        IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
        _productCollection = database.GetCollection<Product>(mongoDBSettings.Value.CollectionName);
    }

    public async Task<List<Product>> GetAsync() {
        return await _productCollection.Find(new BsonDocument()).ToListAsync();
    }

    public async Task CreateAsync(Product product) {
        await _productCollection.InsertOneAsync(product);
        return;
    }
    
    public async Task UpdateAsync(string id, Product product) {
        FilterDefinition<Product> filter = Builders<Product>.Filter.Eq("Id", id);
        var updateDefinition = new List<UpdateDefinition<Product>>();
        
        foreach (var dataField in product.ToBsonDocument()) {
            if(dataField.Value != null && !(dataField.Value is MongoDB.Bson.BsonNull)) {
                updateDefinition.Add(Builders<Product>.Update.Set(dataField.Name, dataField.Value));
            }
        }

        var combinedUpdate = Builders<Product>.Update.Combine(updateDefinition);
        await _productCollection.UpdateManyAsync(filter, combinedUpdate);
        return;
    }

    public async Task DeleteAsync(string id) {
        FilterDefinition<Product> filter = Builders<Product>.Filter.Eq("Id", id);
        await _productCollection.DeleteOneAsync(filter);
        return;
    }
}