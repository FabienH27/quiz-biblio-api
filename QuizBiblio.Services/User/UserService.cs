using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using QuizBiblio.DataAccess.User;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using M = QuizBiblio.Models;

namespace QuizBiblio.Services.User;

public class UserService(IUserRepository userRepository) : IUserService
{
    public async Task<List<M.UserEntity>> GetUsers() => await userRepository.GetUsers();

    public async Task<M.UserEntity?> GetUser(ObjectId id) => await userRepository.GetUser(id);

    public async Task<M.UserEntity?> GetUser(string email) => await userRepository.GetUser(email);

    public async Task Create(M.UserEntity user)
    {
        await userRepository.Create(user);
    }
}
