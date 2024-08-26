using MarketAPI.Database.Models;
using MarketAPI.Models.User;

namespace MarketAPI.Abstraction;

public interface IUserService
{
    Task<User?> GetUserByEmail(string Email);
    Task<User?> GetUser(string Email, string Password);
    Task CreateUser(User user);
    Task<LoginResponse> GenerateToken(User user);
}