using MarketAPI.Models.User;

namespace MarketAPI.Validator;

public class UserLoinValidator : AbstractValidator<LoginRequest>
{
    public UserLoinValidator()
    {
        RuleFor(x => x.Email).EmailAddress();
        RuleFor(x => x.Password).NotEmpty();
    }
}