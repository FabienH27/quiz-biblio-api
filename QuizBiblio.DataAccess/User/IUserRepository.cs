using MongoDB.Bson;
using QuizBiblio.Models;

namespace QuizBiblio.DataAccess.User;

public interface IUserRepository
{
    /// <summary>
    /// Get all users
    /// </summary>
    /// <returns>all users</returns>
    public Task<List<UserEntity>> GetUsersAsync();

    /// <summary>
    /// Get a specific user
    /// </summary>
    /// <param name="username">username of the user to fetch</param>
    /// <returns>specific user</returns>
    public Task<UserEntity?> GetUserAsync(string id);

    /// <summary>
    /// Get a specific user
    /// </summary>
    /// <param name="username">email of the user to fetch</param>
    /// <returns>specific user</returns>
    public Task<UserEntity?> GetUserFromMail(string email);

    /// <summary>
    /// Create a user
    /// </summary>
    /// <param name="user">user to create</param>
    /// <returns>created user</returns>
    public Task CreateAsync(UserEntity user);


    /// <summary>
    /// Grants admin access to given user
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Task GrantAccess(string id);
}
