using MongoDB.Driver;
using QuizBiblio.DataAccess.QbDbContext;
using QuizBiblio.Models.Users;

namespace QuizBiblio.DataAccess.Users;

public class UserRepository(IMongoDbContext dbContext) : IUserRepository
{

    private readonly IMongoDbContext _dbContext = dbContext;

    IMongoCollection<UserEntity> Users => _dbContext.GetCollection<UserEntity>("Users");

    public async Task<List<UserEntity>> GetUsers() => await Users.Find(_ => true).ToListAsync();

    public async Task<UserEntity?> GetUser(string id) => await Users.Find(user => user.Id == id).FirstOrDefaultAsync();

    public async Task<UserEntity?> GetUserFromMail(string email) => await Users.Find(user => user.Email == email).FirstOrDefaultAsync();

    public async Task CreateAsync(UserEntity user)
    {
        await Users.InsertOneAsync(user);
    }
}
