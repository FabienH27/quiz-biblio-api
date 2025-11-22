using QuizBiblio.Models.Users;

namespace QuizBiblio.Services.Users;

public interface IUserService
{

    /// <summary>
    /// Get all users
    /// </summary>
    /// <returns>all users</returns>
    public Task<List<User>> GetUsers();

    /// <summary>
    /// Get a specific user
    /// </summary>
    /// <param name="id">id of the user to fetch</param>
    /// <returns>specific user</returns>
    public Task<User?> GetUser(string id);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    public Task<User?> GetUserFromMail(string username);

    /// <summary>
    /// Create a user
    /// </summary>
    /// <param name="user">user to create</param>
    /// <returns>created user</returns>
    public Task CreateAsync(UserEntity user);
}
