using MongoDB.Bson;
using M = QuizBiblio.Models;

namespace QuizBiblio.Services.User;

public interface IUserService
{

    /// <summary>
    /// Get all users
    /// </summary>
    /// <returns>all users</returns>
    public Task<List<M.User>> GetUsers();

    /// <summary>
    /// Get a specific user
    /// </summary>
    /// <param name="id">id of the user to fetch</param>
    /// <returns>specific user</returns>
    public Task<M.User?> GetUser(ObjectId id);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    public Task<M.User?> GetUser(string username);

    /// <summary>
    /// Create a user
    /// </summary>
    /// <param name="user">user to create</param>
    /// <returns>created user</returns>
    public void Create(M.User user);
}
