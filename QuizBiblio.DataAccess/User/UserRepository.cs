using MongoDB.Driver;
using QuizBiblio.DataAccess.QbDbContext;
using QuizBiblio.Models;
using QuizBiblio.Models.Rbac;

namespace QuizBiblio.DataAccess.User;

public class UserRepository : IUserRepository
{

    private readonly IMongoDbContext _dbContext;

    private readonly FilterDefinitionBuilder<UserEntity> Filters = Builders<UserEntity>.Filter;
    private readonly UpdateDefinitionBuilder<UserEntity> Update = Builders<UserEntity>.Update;


    IMongoCollection<UserEntity> Users => _dbContext.GetCollection<UserEntity>("Users");

    public UserRepository(IMongoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<UserEntity>> GetUsersAsync() => await Users.Find(_ => true).ToListAsync();

    public async Task<UserEntity?> GetUserAsync(string id) => await Users.Find(user => user.Id == id).FirstOrDefaultAsync();

    public async Task<UserEntity?> GetUserFromMail(string email) => await Users.Find(user => user.Email == email).FirstOrDefaultAsync();

    public async Task CreateAsync(UserEntity user)
    {
        await Users.InsertOneAsync(user);
    }

    public async Task GrantAccess(string id)
    {
        var filter = Filters.Eq(user => user.Id, id);

        var update = Update.Set(user => user.UserRole, Role.Admin.Name);

        await Users.UpdateOneAsync(filter, update);
    }
}
