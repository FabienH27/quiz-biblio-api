using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;
using QuizBiblio.DataAccess.QbDbContext;
using QuizBiblio.Models;

namespace QuizBiblio.DataAccess.User;

public class UserRepository : IUserRepository
{

    private readonly IMongoDbContext _dbContext;

    IMongoCollection<UserEntity> Users => _dbContext.GetCollection<UserEntity>("Users");

    public UserRepository(IMongoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<UserEntity>> GetUsers() => await Users.Find(_ => true).ToListAsync();

    public async Task<UserEntity?> GetUser(ObjectId id) => await Users.Find(user => user.Id == id).FirstOrDefaultAsync();

    public async Task<UserEntity?> GetUser(string email) => await Users.Find(user => user.Email == email).FirstOrDefaultAsync();

    public async Task Create(UserEntity user)
    {
        await Users.InsertOneAsync(user);
    }
}
