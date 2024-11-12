using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using QuizBiblio.DataAccess.User;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using M = QuizBiblio.Models;

namespace QuizBiblio.Services.User;

public class UserService(IUserRepository userRepository) : IUserService
{
    public async Task<List<M.User>> GetUsers() => await userRepository.GetUsers();

    public async Task<M.User?> GetUser(ObjectId id) => await userRepository.GetUser(id);

    public async Task<M.User?> GetUser(string email) => await userRepository.GetUser(email);

    public void Create(M.User user)
    {

        userRepository.Create(user);
    }
}
