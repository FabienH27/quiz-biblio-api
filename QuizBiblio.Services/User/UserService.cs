using QuizBiblio.DataAccess.User;
using M = QuizBiblio.Models;

namespace QuizBiblio.Services.User;

public class UserService(IUserRepository userRepository) : IUserService
{
    public async Task<List<M.UserEntity>> GetUsersAsync() => await userRepository.GetUsersAsync();

    public async Task<M.UserEntity?> GetUserAsync(string id) => await userRepository.GetUserAsync(id);

    public async Task<M.UserEntity?> GetUserFromMail(string email) => await userRepository.GetUserFromMail(email);

    public async Task CreateAsync(M.UserEntity user) => await userRepository.CreateAsync(user);

    public Task GrantAccess(string id) => userRepository.GrantAccess(id);
}
