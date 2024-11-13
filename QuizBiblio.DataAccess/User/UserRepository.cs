using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;

namespace QuizBiblio.DataAccess.User;

public class UserRepository(QuizBiblioDbContext dbContext) : IUserRepository
{
    DbSet<Models.User> Users => dbContext.Users;

    public async Task<List<Models.User>> GetUsers() => await Users.ToListAsync();

    public async Task<Models.User?> GetUser(ObjectId id) => await Users.FirstOrDefaultAsync(user => user.Id == id);

    public async Task<Models.User?> GetUser(string email) => await Users.FirstOrDefaultAsync(user => user.Email == email);

    //Non async : standard use case, refer to Add() method documentation
    public void Create(Models.User user)
    {
        Users.Add(user);
        dbContext.SaveChanges();
    }
}
