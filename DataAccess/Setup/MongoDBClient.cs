using DataAccess.Models;
using DataAccess.Options;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace DataAccess.Setup;

public class MongoDBClient
{
    private readonly MongoClient _client;
    private readonly string _databaseName;
    private readonly string _collectionName;

    public MongoDBClient(IOptions<MongoDBSettings> options)
    {
        var testConnection = Environment.GetEnvironmentVariable("Test_MongoDbConnectionString");

        if (!string.IsNullOrEmpty(testConnection))
        {
            _client = new MongoClient(testConnection);
        }
        else
        {
            _client = new MongoClient(options.Value.ConnectionString);
        }

        _databaseName = options.Value.DatabaseName;
        _collectionName = options.Value.CollectionName;
    }
    

    public IMongoCollection<Office> GetOfficeCollection()
    {
        var db = _client.GetDatabase(_databaseName);

        return db.GetCollection<Office>(_collectionName);
    }
}