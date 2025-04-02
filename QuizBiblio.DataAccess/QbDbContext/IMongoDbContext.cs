using MongoDB.Driver;

namespace QuizBiblio.DataAccess.QbDbContext;

public interface IMongoDbContext
{
    IMongoCollection<T> GetCollection<T>(string collectionName);
}
