namespace MarketAPI.Models.User;

public record UserRequest(string Name, string Email, string Password, string Role);