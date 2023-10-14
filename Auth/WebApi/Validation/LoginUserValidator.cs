using FluentValidation;

namespace WebApi.Validation;

public class LoginUserValidator : AbstractValidator<LoginUser>
{
    public LoginUserValidator()
    {
        RuleFor(l => l.Email).EmailAddress().NotNull();
        RuleFor(l => l.Password).NotNull();
    }
}
