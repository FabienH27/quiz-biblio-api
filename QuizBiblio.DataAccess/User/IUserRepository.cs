using MongoDB.Bson;
using QuizBiblio.Models;

namespace QuizBiblio.DataAccess.User;

public interface IUserRepository
{
    /// <summary>
    /// Get all users
    /// </summary>
    /// <returns>all users</returns>
    public Task<List<UserEntity>> GetUsers();

    /// <summary>
    /// Get a specific user
    /// </summary>
    /// <param name="username">username of the user to fetch</param>
    /// <returns>specific user</returns>
    public Task<UserEntity?> GetUser(ObjectId id);

    /// <summary>
    /// Get a specific user
    /// </summary>
    /// <param name="username">email of the user to fetch</param>
    /// <returns>specific user</returns>
    public Task<UserEntity?> GetUser(string email);

    /// <summary>
    /// Create a user
    /// </summary>
    /// <param name="user">user to create</param>
    /// <returns>created user</returns>
    public Task Create(UserEntity user);
}
