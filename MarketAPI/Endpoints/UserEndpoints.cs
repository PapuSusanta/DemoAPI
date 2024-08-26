
using MarketAPI.Abstraction;
using MarketAPI.Database.Models;
using MarketAPI.Filters;
using MarketAPI.Models.User;
using Microsoft.AspNetCore.Http.HttpResults;

namespace MarketAPI.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        var userGroup = app.MapGroup("/user");

        userGroup.MapPost("/login", GetUser)
            .AddEndpointFilter<ValidationFilter<LoginRequest>>();
        userGroup.MapGet("/{Email}", GetUserByEmail);
        userGroup.MapPost("/register", CreateUser)
            .AddEndpointFilter<ValidationFilter<UserRequest>>();
    }

    private static async Task<Results<NoContent, ProblemHttpResult>> CreateUser(IUserService userService, UserRequest request)
    {
        var userId = Guid.NewGuid();
        var user = new User(userId.ToString(), request.Name, request.Email, request.Password, request.Role);
        await userService.CreateUser(user);
        return TypedResults.NoContent();
    }

    private static async Task<Results<Ok<UserResponse>, ProblemHttpResult>> GetUserByEmail(IUserService userService, string Email)
    {
        var user = await userService.GetUserByEmail(Email);
        if (user is null)
        {
            return TypedResults.Problem(statusCode: 404, title: "User not found");
        }

        return TypedResults.Ok(new UserResponse(user.Id, user.Name, user.Email, user.Role));
    }

    private static async Task<Results<Ok<LoginResponse>, ProblemHttpResult>> GetUser(IUserService userService, LoginRequest request)
    {
        var user = await userService.GetUser(request.Email, request.Password);
        if (user is null)
        {
            return TypedResults.Problem(statusCode: 404, title: "User not found");
        }
        // return TypedResults.Ok(new UserResponse(user.Id, user.Name, user.Email, user.Role));

        var response = await userService.GenerateToken(user);

        return TypedResults.Ok(response);
    }
}