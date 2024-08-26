using MarketAPI.Models.User;

namespace MarketAPI.Validator;

public class UserValidator : AbstractValidator<UserRequest>
{
    public UserValidator()
    {
        RuleFor(u => u.Name).NotEmpty();
        RuleFor(u => u.Email).EmailAddress();
        RuleFor(u => u.Password).NotEmpty();
        RuleFor(u => u.Role).NotEmpty();
    }
}