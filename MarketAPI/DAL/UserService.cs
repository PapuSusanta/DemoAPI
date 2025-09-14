using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Dapper;
using MarketAPI.Abstraction;
using MarketAPI.Database.Models;
using MarketAPI.Models.User;
using Microsoft.IdentityModel.Tokens;

namespace MarketAPI.DAL;

public class UserService(IDbConnection connection) : IUserService
{
    private readonly IDbConnection _connection = connection;

    public async Task CreateUser(User user)
    {
        string sql = """
        INSERT INTO user (Id, Name, Email, Password, Role)
        VALUES (@Id, @Name, @Email, @Password, @Role)
        """;

        await _connection.ExecuteAsync(sql, user);
    }

    public async Task<User?> GetUserByEmail(string Email)
    {
        string sql = """
        SELECT 
            Id, Name, Email, Password, Role 
        FROM 
            user
        WHERE 
            Email = @Email
        """;
        var user = await _connection.QueryFirstOrDefaultAsync<User>(sql, new { Email });
        return user;
    }

    public Task<User?> GetUser(string Email, string Password)
    {
        string sql = """
        SELECT 
            Id, Name, Email, Password, Role 
        FROM 
            user
        WHERE 
            Email = @Email AND Password = @Password
        """;
        var user = _connection.QueryFirstOrDefaultAsync<User>(sql, new { Email, Password });
        return user;
    }

    public Task<LoginResponse> GenerateToken(User user)
    {

        // please give me a 256 bits key
        var key = "6f47cdb3d8b17362924e92f6a8d2761f72076d8c46a7242f346d9924581c2e69";

        var credential = new SigningCredentials(
                        new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(key)),
                        SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.Email),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim(ClaimTypes.NameIdentifier, user.Id)
        };
        var securityToken = new JwtSecurityToken(
            issuer: "MarketAPI",
            audience: "MarketAPI",
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(10),
            signingCredentials: credential
        );
        var token = new JwtSecurityTokenHandler().WriteToken(securityToken);
        var refreshToken = Guid.NewGuid().ToString();
        return Task.FromResult(new LoginResponse(user.Email, token, refreshToken));
    }
}