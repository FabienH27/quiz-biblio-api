using AutoMapper;
using QuizBiblio.DataAccess.Users;
using QuizBiblio.Models.Users;

namespace QuizBiblio.Services.Users;

public class UserService(IUserRepository userRepository, IMapper mapper) : IUserService
{
    public async Task<List<User>> GetUsers() {
        var entities = await userRepository.GetUsers();

        return mapper.Map<List<User>>(entities);
    }

    public async Task<User?> GetUser(string id) {
        var entity = await userRepository.GetUser(id);
        return mapper.Map<User?>(entity);
    }

    public async Task<User?> GetUserFromMail(string email) {
        var entity = await userRepository.GetUserFromMail(email);
        return mapper.Map<User?>(entity);
    } 

    public async Task CreateAsync(UserEntity user)
    {
        await userRepository.CreateAsync(user);
    }
}
