using MongoDB.Bson;
using QuizBiblio.Models;

namespace QuizBiblio.Services.User;

public interface IUserService
{

    /// <summary>
    /// Get all users
    /// </summary>
    /// <returns>all users</returns>
    public Task<List<UserEntity>> GetUsersAsync();

    /// <summary>
    /// Get a specific user
    /// </summary>
    /// <param name="id">id of the user to fetch</param>
    /// <returns>specific user</returns>
    public Task<UserEntity?> GetUserAsync(string id);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    public Task<UserEntity?> GetUserFromMail(string username);

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
