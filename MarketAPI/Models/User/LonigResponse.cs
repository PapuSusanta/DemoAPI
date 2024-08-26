namespace MarketAPI.Models.User;

public record LoginResponse(string Email, string Token, string RefreshToken);