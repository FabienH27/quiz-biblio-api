using Microsoft.Extensions.Options;
using MongoDB.Driver;
using QuizBiblio.Models.DatabaseSettings;

namespace QuizBiblio.DataAccess.QbDbContext;

public class MongoDbContext : IMongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(IMongoClient mongoClient, IOptions<QuizStoreDatabaseSettings> options)
    {
        var databaseName = options.Value.DatabaseName;
        _database = mongoClient.GetDatabase(databaseName);

    }

    public IMongoCollection<T> GetCollection<T>(string collectionName)
    {
        return _database.GetCollection<T>(collectionName);
    }
}
