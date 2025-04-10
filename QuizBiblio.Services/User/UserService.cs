using MongoDB.Bson;
using QuizBiblio.DataAccess.User;
using M = QuizBiblio.Models;

namespace QuizBiblio.Services.User;

public class UserService(IUserRepository userRepository) : IUserService
{
    public async Task<List<M.UserEntity>> GetUsers() => await userRepository.GetUsers();

    public async Task<M.UserEntity?> GetUser(string id) => await userRepository.GetUser(id);

    public async Task<M.UserEntity?> GetUserFromMail(string email) => await userRepository.GetUserFromMail(email);

    public async Task CreateAsync(M.UserEntity user)
    {
        await userRepository.CreateAsync(user);
    }
}
